import nmrglue as ng
import os
import struct
import sys
from shutil import copytree

# 1. get passed folder name of FID as sys.argv[1]
# 2. copy fid, procpar, text, etc. to new folder '../name_backup'
# 3. read fileheader, data from fid file in '../_backup'
# 4. change fileheader to correct information
# 5. write fileheader, data to fid file in folder (overwrite)


def fixBA_Model_FIDs(FID_Folder):
    # check if fixed
    basedir = os.path.dirname(FID_Folder)
    backup_dir = str(os.path.basename(FID_Folder)) + '_backup'
    backup_dir = os.path.join(basedir, backup_dir)
    with open(os.path.join(FID_Folder, 'fid'), 'rb') as f:
        header = struct.unpack('>6lhhl', f.read(32))
        newdic = ng.varian.fileheader2dic(header)
        if newdic['nbheaders'] == 1:
            return 1
        filedata = f.read()
    if not os.path.exists(backup_dir):
        copytree(FID_Folder, backup_dir)
    newdic['nbheaders'] = 1
    # new FID location:
    newFID = os.path.join(FID_Folder, 'fid')
    # remove the existing fid from the folder
    if os.path.exists(newFID):
        os.remove(newFID)
    # write the new, fixed FID
    fixedFileHeader = ng.varian.dic2fileheader(newdic)
    with open(newFID, 'wb') as f:
        ng.fileio.varian.put_fileheader(f, fixedFileHeader)
        f.write(filedata)


if __name__ == '__main__':
    fixBA_Model_FIDs(sys.argv[1])
