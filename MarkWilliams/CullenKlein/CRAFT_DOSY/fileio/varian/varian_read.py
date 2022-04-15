import os
import nmrglue as ng
import numpy as np
import matplotlib.pyplot as plt


class NMRExpObject:
    pass


class VarianExp(NMRExpObject):
    
    VN_FIDFILE = 'fid'
    VN_PARFILE = 'procpar'
    VN_HEADERSIZE = 28
    
    def __init__(self, VarianFolder):
        self.procpar, self.data = read_varian_folder(VarianFolder)       
         

def read_varian_folder(folder):
    try:
        procpar, data = ng.varian.read(dir=folder)
    except IOError:
        print('fid_path does not specify a valid .fid directory:', folder)
    return (procpar, data)


class Varian_1H1D(VarianExp):
    
    def __init__(self, procpar, data):
        super.__init__(self, procpar, data)
        self.params = self.make_params_dict()
        self.procpar = self.convert_procpar_to_true_dict()
        self.SpectralDetail = self.CreateSpecDetailDic()
        
    def make_params_dict(self):
        list1 = ['at', 'd1', 'sfrq', 'reffrq', 'rfp', 'rfl', 'tof', 'sw', 'gain', 'pw90', 'ref_pw90', 'pw', 'tpwr', 'temp']
        list2 = ['explist', 'explabel', 'studyowner', 'systemname_', 'samplename', 'solvent', 'filename', 'time_run']
        procpar = self.procpar
        params = {x: float(procpar['procpar'][x]['values'][0]) for x in list1}
        params.update({x: procpar['procpar'][x]['values'][0] for x in list2})
        params['rt'] = params['at'] + params['d1']
        params['nt'] = int(procpar['procpar']['nt']['values'][0])
        params['arraydim'] = int(procpar['procpar']['arraydim']['values'][0])
        params['acqtime'] = (params['nt']*params['rt'])/60.  # convert to mins.
        params['sw_ppm'] = round(params['sw']/params['reffrq'], 2)
        sfrq = params['sfrq']
        sw = params['sw']
        params['sw_left'] = (0.5+1e6*(sfrq-params['reffrq'])/sw)*sw/sfrq
        params['system'] = params['systemname_'].split('.')[0]
        params['ppm_lt'] = params['sw_left']
        params['ppm_rt'] = params['sw_left'] - params['sw_ppm']
        params['tot_np'] = int(procpar['procpar']['np']['values'][0])
        params['real_np'] = int(params['tot_np'] / 2)
        params['time_run_str'] = '_'.join(params['time_run'].split('T'))
        params['time_run_dec'] = int(''.join(params['time_run'].split('T')))
        return params

    def convert_procpar_to_true_dict(self):
        procpar = self.procpar
        procpar_dict = {elem: procpar['procpar'][elem]['values'][0] for
                        elem in procpar['procpar']}
        return procpar_dict
    
    def CreateSpecDetailDic(self):
        Detaildic = dict()
        key_list = ['at', 'd1', 'sfrq', 'reffreq', 'rfp', 'rfl', 'tof', 'rt', 'nt', 'acqtime', 'sw', 'sw_ppm', 'sw_left', 'ppm_lt', 'ppm_rt', 'tot_np', 'real_np', 'gain', 'pw90', 'solvent']
        Detaildic = {key: self.params['key'] for key in key_list}
        return Detaildic
    
    ef default_plt_time(self):
        t_sc = np.arange(0, self.params['at'], self.params['at']/len(self.data))
        fig = plt.figure()
        ax = fig.add_subplot(111)
        ax.plot(t_sc, self.data, 'k-', lw=0.15)
        ax.set_yticklabels([])
        ax.set_title(str('1D Spectrum - ' + self.params['sample_name']))
        ax.set_xlabel("sec")
        ax.set_xlim(0, self.params['at'])
        ax.xaxis.set_minor_locator(AutoMinorLocator())
        ax.tick_params(which='minor', length=4)
        plt.show()

    def default_plt_freq(self):
        ndata = [ng.process.proc_base.fft(i) for i in self.data]
        ph_data = [ng.proc_autophase.autops(i, fn='acme') for i in ndata]
        ppm_lt = self.params['ppm_lt']
        ppm_rt = self.params['ppm_rt']
        ppms = np.arange(ppm_lt, ppm_rt, -((ppm_lt - ppm_rt)/len(ndata)))
        fig = plt.figure()
        ax = fig.add_subplot(111)
        for data in ph_data:
            ax.plot(ppms, data, 'k-', lw=0.15)
        ax.set_yticklabels([])
        ax.set_title(str('1D Spectrum - ' + self.params['sample_name']))
        ax.set_xlabel("1H ppm")
        ax.set_xlim(np.max(ppms), np.min(ppms))
        ax.xaxis.set_minor_locator(AutoMinorLocator())
        ax.tick_params(which='minor', length=4)
        plt.show()