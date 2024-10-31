This assignments recursively opens all the directories and csv files in each directory, goes through all the csv file rows, logs the incomplete/invalid lines as skipped rows, and prints the valid csv file rows into a new Output.csv file.

The main function calls the ListDirectory function, which is a recursive function. It recursively opens the directories, and calls the ReadCsvFile function which reads each row on the csv files, and writes good rows to the Output.csv fle. 

The WriteToLogFile function write every info and error messages to the log file. 

The IsValid, IsValidEmail, and containSpecialCharacter functions detects the invalid rows from the csv files.

The rootdirectory (the path to the root directory), output file path, and the log file path are hard coded. They will have to changed to run the code on a different machine. 