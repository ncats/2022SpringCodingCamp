import os
import nmrglue as ng
import numpy as np
import matplotlib.pyplot as plt
import varian1H1D_intake as v1Di
import scipy.integrate
import re
import pendulum
import BAModelStack_Fix as BAMSF



'''****Configs****'''
BAFolder = 'BayesAnalyzeFiles'
Modelfile = 'bayes.summary1.0001'
'''
This module intakes the Bayes Analyze Results from  Varian 1H 1D spectra folder
and processes it into a data object.  A calc Uniq ID will be generated.
'''

'''
Calculation Metadata
    - FID Index
    - BayesJobID
    - Calc Type (eg. Standard vs Big Pk/Little Pk etc)
    - Number of resonances identified on FID
    - table name for where record of exp details will be stored (first nowhere)

Basic CRAFT Peak Calc Data
    - CalcUniqID
    - Peaks
    - RegionID
    - Pointer to resonance model FID
    - NMR Exp type

'''


class Bayes1H1DResult:

    def __init__(self, resultdir, SpectralUniqID):
        self.resultdir = resultdir
        self.calctype = 'CRAFT_Standard_1D'
        self.summarydict = self.ParseSummaryHeader()
        self.FIDIndex = 1
        self.CalcDestinationDataTable = 'CRAFT_Standard_1D_Resonances'
        self.BayesJobID = self.summarydict['jobID']
        # dt_str = self.summarydict['datetime'].split(maxsplit=2)[2].strip()
        self.modelcomponents = self.ParseBayesSummary()
        self.number_resonances = sum(isinstance(i, Resonance) for i in self.modelcomponents)
        self.DetailsTable = 'CRAFT_Standard_1D_Model'
        self.modelfids = BAModel_Arrayed(self.CalcUniqID, self.resultdir)

    def ParseSummaryHeader(self):
        BayesSummaryFile = os.path.join(self.resultdir, BAFolder, Modelfile)
        with open(BayesSummaryFile, 'r') as fp:
            lines = [line.strip() for line in fp.readlines()]
        dt_str = ' '.join(lines[1].split(maxsplit=2)[2].strip().split())
        dt = pendulum.from_format(dt_str, 'MMM D HH:mm:ss YYYY')
        d = dt.format('YYYYMMDD_HHmmss')
        bayesFilePath = lines[3].split()[1]
        jobID = bayesFilePath[12:18]
        # Some files have the following line indices for their values
        # For some reason others have an extra line
        filepathline = 8
        configParamNosline = 10
        lineIndex = 12
        # testing for extra line
        if lines[filepathline][0] == '-':
            filepathline += 1
            configParamNosline += 1
            lineIndex += 1
        FIDFilePath = lines[filepathline].split(maxsplit=2)[2]
        configParamNos = int(lines[configParamNosline].split()[0])
        paramIndex = 0
        paramDict = {}
        numPattern = re.compile(r'[-+]?\d*\.?\d+|[-+]?\d+')  # looks for number
        decPattern = re.compile(r'\.')  # looks for decimal point, i.e. float
        while paramIndex < configParamNos:
            [key, value] = [x.strip() for x in
                            lines[lineIndex+paramIndex].split('=')]
            if numPattern.search(value):
                if decPattern.search(value):
                    value = float(value)
                else:
                    value = int(value)
            paramDict[key] = value
            paramIndex += 1
        paramDict.update({'jobID': jobID, 'FIDFilePath': FIDFilePath, 'bayesFilePath': bayesFilePath, 'DateTime': d})
        return paramDict

    def ParseBayesSummary(self):
        BayesSummaryFile = os.path.join(self.resultdir, BAFolder, Modelfile)
        with open(BayesSummaryFile, 'r') as fp:
            lines = [line.strip() for line in fp.readlines()]
        index = 20  # Skip the config parameters section
        modelcomponents = []
        while index < len(lines):
            # find the header divider line '------...----'
            if lines[index].split(maxsplit=3)[0:2] == ['#', 'Name']:
                index += 1
                lineA = lines[index].split()
                if lineA[1] == 'Real':
                    RC = ParseConstantFactor(lines[index:index+4])
                    modelcomponents.append(RealConstant(*RC))
                    index += 4
                elif lineA[1] == 'Imaginary':
                    IC = ParseConstantFactor(lines[index:index+4])
                    modelcomponents.append(ImaginaryConstant(*IC))
                    index += 4
                else:
                    correlation = lineA[1]
                    multiplicity = lineA[2]
                    if multiplicity == 'Singlet':
                        if correlation == '(CP)':
                            CPS = ParseCPSinglet(lines[index:index+6])
                            modelcomponents.append(CPS)
                        else:
                            UPS = ParseUPSinglet(lines[index:index+6])
                            modelcomponents.append(UPS)
                        index += 5
                    else:
                        raise(NotImplementedError,
                              "Multiplet parsing not yet implemented")
            index += 1
        return modelcomponents

    def decay_rate(self, index):
        alpha = self.modelcomponents[index].decay
        no_points = self.summarydict['Total Points']
        delta_t = self.summarydict['Sampling Time'] / no_points
        fwhm = alpha/np.pi  # (np.pi * self.summarydict['Spec Freq'] * 1000000)
        # decay_rate = fwhm * np.pi() * delta_t / no_points
        return fwhm

    def Lorentzian(self, f_z, d_t, a_pk):
        return lambda x: (a_pk*d_t)/(2*np.pi*(x-f_z)**2 + a_pk**2)

    def ppm_to_Hz(self, ppm, field, ref_freq):
        f = field * ppm - ref_freq
        return f

    def hz_to_ppm(self, freq, field, ref_freq):
        ppm = (freq - ref_freq) / field
        return ppm

   
    def sorted_dist_list(self):
        return sorted(self.peak_dist_cross())


class BAModel_Arrayed(v1Di.VN_Spectra_Obj):

    def __init__(self, CalcUniqID, CalcDirectory):
        BA_model_fldr = os.path.join(CalcDirectory, 'bayes.model.fid')
        BAMSF.fixBA_Model_FIDs(BA_model_fldr)
        super().__init__(*v1Di.read_varian_folder(BA_model_fldr))
        self.CalcUniqID = CalcUniqID
        self.cmpltmodel = self.data[1]
        self.cmpltfft = ng.process.proc_base.fft(self.cmpltmodel)
        self.cmpltfft = ng.proc_autophase.autops(self.cmpltfft, fn='acme', disp=0)
        self.residualmodel = self.data[2]
        self.residualfft = ng.process.proc_base.fft(self.residualmodel)
        self.residualfft = ng.proc_autophase.autops(self.residualfft, fn='acme', disp=0)
        self.spectrum = self.data[0]
        self.specfft = ng.process.proc_base.fft(self.spectrum)
        self.specfft = ng.proc_autophase.autops(self.specfft, fn='acme', disp=0)
        self.data = self.data[3:]
        self.datafft = [ng.process.proc_base.fft(i) for i in self.data]
        self.datafft = [ng.proc_autophase.autops(i, fn='acme', disp=0) for i in self.datafft]

    def plot_models_overlay(self, resonance_indices, xlim, cmplt_model=False, residual=False, orig_spectra=False):
        ppm_lt = self.params['ppm_lt']
        ppm_rt = self.params['ppm_rt']
        ppms = np.arange(ppm_lt, ppm_rt, -((ppm_lt - ppm_rt)/len(self.datafft[1])))
        fig = plt.figure()
        ax = fig.add_subplot(111)
        ct = len(resonance_indices) + sum([cmplt_model, residual, orig_spectra])
        for i in resonance_indices:
            ax.plot(ppms, self.datafft[i], 'k-', lw=0.15)
        if cmplt_model:
            ax.plot(ppms, self.cmpltfft, 'k-', lw=0.5)
        if residual:
            ax.plot(ppms, self.residualfft, 'k-', lw=0.5)
        if orig_spectra:
            ax.plot(ppms, self.specfft, 'k-', lw=0.5)
        # ax.set_yticklabels([])
        ax.set_title(str('1D Spectrum - ' + self.params['samplename']))
        ax.set_xlabel("1H ppm")
        if not xlim:
            ax.set_xlim(np.max(ppms), np.min(ppms))
        else:
            ax.set_xlim(*xlim)
        ax.tick_params(which='minor', length=4)
        plt.show()

    def model_residual_analysis(self, integr='Simpson'):
        if integr == 'Trapezoid':
            area_spec = np.trapz(self.specfft)
            area_res = np.trapz(np.abs(self.residualfft))
            area_model = np.trapz(self.cmpltfft)
        if integr == 'Simpson':
            area_spec = scipy.integrate.simps(self.specfft)
            area_res = scipy.integrate.simps(np.abs(self.residualfft))
            area_model = scipy.integrate.simps(self.cmpltfft)
        model_spec_frac = area_model/area_spec
        res_spec_frac = area_res/area_spec
        return (res_spec_frac, model_spec_frac)

    def subset_fft(self, whichfft, ppmleft, ppmright):
        if isinstance(whichfft, int):
            fftarray = self.datafft[whichfft]
        elif whichfft == 'Model':
            fftarray = self.cmpltfft
        elif whichfft == 'Residual':
            fftarray = self.residualfft
        elif whichfft == 'Original':
            fftarray == self.specfft
        ppmlt = self.params['ppm_lt']
        ppmrt = self.params['ppm_rt']
        if ppmright < ppmrt:
            ppmright = ppmrt
        if ppmleft > ppmlt:
            ppmleft - ppmlt
        delta_ppm = (len(fftarray) - 1) / (ppmlt - ppmrt)
        start_index = int((ppmright-ppmrt)/delta_ppm)
        stop_index = int((ppmleft-ppmrt)/delta_ppm)
        # if indexing is reversed in array not just in plotting
        # then whichfft[-start_index, -stop_index]
        return (whichfft[start_index, stop_index],
                ppmleft, ppmright, delta_ppm)

    def residual_in_region(self, calc, ppmlft, ppmrgt):
        fftarray = self.subset_fft('Residual', ppmlft, ppmrgt)
        if calc is None or calc == 'Simpson':
            return scipy.integrate.simps(np.abs(fftarray))
        if calc == 'Trapezoid':
            return np.trapz(np.abs(fftarray))

    def overlap_in_region(self, index1, index2, ppmlt, ppmrt):
        array1 = self.subset_fft(index1, ppmlt, ppmrt)
        array2 = self.subset_fft(index2, ppmlt, ppmrt)
        minarray = [min(array1[i], array2[i]) for i in range(len(array1))]
        return scipy.integrate.simps(minarray)


class ModelComponent:
    pass


class Resonance(ModelComponent):
    pass


class UPSinglet(Resonance):

    def __init__(self, fid, freq, freqsigma, decay, decaysigma, amp, ampsigma, phase, phasesigma):
        self.fid = fid
        self.freq = freq
        self.freqsigma = freqsigma
        self.decay = decay
        self.decaysigma = decaysigma
        self.amp = amp
        self.ampsigma = ampsigma
        self.phase = phase
        self.phasesigma = phasesigma
        self.type = 'UPSinglet'

    def TranslateToDict(self):
        trlt = {'fid': self.fid,
                'freq': self.freq, 'freqsigma': self.freqsigma,
                'decay': self.decay, 'decaysigma': self.decaysigma,
                'amp': self.amp, 'ampsigma': self.ampsigma,
                'phase': self.phase, 'phasesigma': self.phasesigma,
                'parametertype': self.type}
        return trlt


class CPSinglet(Resonance):

    def __init__(self, fid, freq, freqsigma, decay, decaysigma, amp, ampsigma):
        self.fid = fid
        self.freq = freq
        self.freqsigma = freqsigma
        self.decay = decay
        self.decaysigma = decaysigma
        self.amp = amp
        self.ampsigma = ampsigma
        self.type = 'CPSinglet'

    def TranslateToDict(self):
        trlt = {'fid': self.fid,
                'freq': self.freq, 'freqsigma': self.freqsigma,
                'decay': self.decay, 'decaysigma': self.decaysigma,
                'amp': self.amp, 'ampsigma': self.ampsigma,
                'parametertype': self.type}
        return trlt

class Constant(ModelComponent):
    
    def __init__(self, fid, amp, ampsigma):
        self.fid = fid
        self.amp = amp
        self.ampsigma = ampsigma
        
    def TranslateToDict(self):
        trlt = {'fid': self.fid,
                'amp': self.amp, 'ampsigma': self.ampsigma,
                'parametertype': self.parameterType}
        return trlt

    
class RealConstant(Constant):

    def __init__(self, fid, amp, ampsigma):
        super().__init__(fid, amp, ampsigma)
        self.parameterType = 'RealConstant'



class ImaginaryConstant(Constant):

    def __init__(self, fid, amp, ampsigma):
        super().__init__(fid, amp, ampsigma)
        self.parameterType = 'ImaginaryConstant'


def ParseConstantFactor(lines):
    lineD = lines[3].split()
    fid = lineD[0]
    amp = float(lineD[1])
    ampsigma = float(lineD[2])
    return [fid, amp, ampsigma]


def ParseUPSinglet(lines):
    lineA = lines[0].split()
    # modelcomponent = int(lineA[0])
    freq, freqsigma = float(lineA[6]), float(lineA[7])
    lineB = lines[1].split()
    decay, decaysigma = float(lineB[1]), float(lineB[2])
    lineE = lines[4].split()
    fid, amp, ampsigma = int(lineE[0]), float(lineE[1]), float(lineE[2])
    phase, phasesigma = float(lineE[3]), float(lineE[4])
    UPSingletfound = UPSinglet(fid=fid, freq=freq, freqsigma=freqsigma, decay=decay, decaysigma=decaysigma, amp=amp, ampsigma=ampsigma,
                               phase=phase, phasesigma=phasesigma)
    return UPSingletfound


def ParseCPSinglet(lines):
    lineA = lines[0].split()
    freq, freqsigma = float(lineA[6]), float(lineA[7])
    lineB = lines[1].split()
    decay, decaysigma = float(lineB[1]), float(lineB[2])
    lineE = lines[4].split()
    fid, amp, ampsigma = int(lineE[0]), float(lineE[1]), float(lineE[2])
    CPSingletfound = CPSinglet(fid=fid, freq=freq, freqsigma=freqsigma, decay=decay, decaysigma=decaysigma, amp=amp, ampsigma=ampsigma)
    return CPSingletfound
