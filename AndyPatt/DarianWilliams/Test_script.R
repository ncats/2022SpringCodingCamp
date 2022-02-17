devtools::load_all("ALTRE")
csvfile <- loadCSVFile("ALTREsampledata/DNaseEncodeExample.csv")
csvfile

samplePeaks <- loadBedFiles(csvfile)
samplePeaks

consensusPeaks <- getConsensusPeaks(samplepeaks = samplePeaks,
                                    minreps = 2)
plotConsensusPeaks(samplepeaks = consensusPeaks)
