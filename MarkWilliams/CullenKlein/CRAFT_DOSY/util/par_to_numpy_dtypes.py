import numpy as np
import pandas as pd
import pickle
import os


'''******Configurations******'''

# in numpy dtype('O') refers to Object (i.e. string)

JobsDtypes = {
    'O': ['jobID', 'Activate Delay', 'Activate Shims', 'Analysis Dir',
          'bayesFilePath', 'Data Type', 'Default Model', 'FIDFilePath',
          'Fid File', 'Model Dir', 'Model Dir Org', 'Noise', 'Output',
          'Procpar File', 'Units'],

    'float64': ['Default Lb', 'File Version', 'Sampling Time', 'Spec Freq',
                'True Reference', 'User Reference'],

    'int64': ['Complex Points', 'First Fid', 'Last Fid', 'Max Candidates',
              'Max Freqs', 'Model Fid', 'Model Points', 'No Fids',
              'Noise Start', 'Prior Odds', 'Shim Order', 'Total Models',
              'Total Points']
            }

ModelDtypes = {
    'O': ['fid', 'jobID', 'parameterType'],

    'float64': ['amp', 'ampsigma', 'decay', 'decaysigma', 'freq',
                'freqsigma', 'phase', 'phasesigma']
              }


SpecsDtypes = {
    'O': ['time_run_str', 'acqtime', 'fidFile_name', 'sample',
          'sample_name', 'time_run'],
    'float64': ['at', 'd1', 'ppm_lt', 'ppm_rt', 'pw90', 'reffrq', 'rfl',
                'rfp', 'rt', 'sfrq', 'sw', 'sw_hz', 'sw_left'],
    'int64': ['nt', 'time_run_dec', 'real_np', 'tot_np']
              }


def MakeDataTypeDict(dict):
    dtypeDict = {}
    for key in dict.keys():
        for colname in dict[key]:
            dtypeDict[colname] = np.dtype(key)
    return dtypeDict

def csv_to_frames(csv_file, df_dtypes):
    return pd.read_csv(csv_file, dtype=df_dtypes, index_col=True)
