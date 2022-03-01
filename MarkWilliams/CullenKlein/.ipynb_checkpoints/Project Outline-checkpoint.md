Coding Camp Project

# Personal Python Skill and Programming Development Goals

1. Git
2. Unit Testing
3. IDE setup
4. User Interface (PyQT5? Plotly/Dash? JupyterWidgets?)

Stretch Goals
1. Advanced Data Graphics
2. SQL/Database utilization
3. OOP


# Specific Project Goals

## C Goals

1. Take a BA DOSY Input
  *  a. Create Test Data folder and place 1D and DOSY data inside
  *  b. create project file directory structure
2. Ingest Data
    a. read data file
    b. parse into data structures
3. Perform DOSY Fit on all Peaks w/ sufficient data
    a. determine the requirements of data for good fitting
    b. add uncertainity values to fit
    c. read in other parameter values for the experiment
    d. perform fits
    e. raise errors as appropriate
    f. create fit data structures with appropriate uncertainties in fit
4. Plot DOSY 2D digitized
    a. create plot from data structures in point form
    b. create peak shapes on points
    c. calculate estimated MW
    d. with mouseover show estimated MW
5. Allow user demarcated "separation"
    a. add widget for user inputted horizontal line
    b. split data set based on horizontal line call back
    c. create reconstructed plot of single peak based on estimated values 
6. Collect peaks into digital sets representing the species
    a. combine multiple estimated peaks into single reconstructed plot
    b. create multiple plots based on 1 or more dividing lines
7. Plot resulting synthetic 1D spectra for separated species

## B Goals
1. Take a 1D digitized spectra and do full coupling analysis
2. match peaks of same J values for multiplets
3. check relative magnitudes of peaks within 24Hz (for 1H)

## A Goals
Submit single job via POST to Bayesian Server
Submit batch job to Bayesian Server
