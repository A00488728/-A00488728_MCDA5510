using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;


namespace ConsoleApp1
{
    internal class Program
    {
        static int number_of_invalid_rows = 0; // counts the number of invalid rows
        static int number_of_valid_rows = 0; // counts the number of valid rows
        static int number_of_rows = 0; // indicates the current row number on each csv file

        static string info = ""; // info message on the log file

        static string date = ""; // date 

        static string rootDirectory = @"C:\Users\0B7744649\Downloads\Sample Data\Sample Data"; // rootDirectory path
        static string outputfilePath = @"C:\Users\0B7744649\source\repos\ConsoleApp1\ConsoleApp1\Output\Output.csv"; // output file path
        static void Main(string[] args)
        {
            string[] headers = { "First Name", "Last Name", "Street Number", "Street", "City", "Province", "Country", "Postal Code", "Phone Number", "Email Address", "Date" }; // Header for the output file
            
            Stopwatch stopwatch = new Stopwatch(); 
            stopwatch.Start(); // stop watch begins
            using (StreamWriter writer = new StreamWriter(outputfilePath))
            {
                writer.WriteLine(string.Join(",", headers)); //  write the header on the output file
            }


                ListDirectories(rootDirectory); // call the ListDirectories function which recursively opens all the directories

            stopwatch.Stop(); // stopwatch watch stops

            TimeSpan ts = stopwatch.Elapsed; // calculate the time of execution

            string time_taken= $"Time taken for execution: {ts.TotalSeconds} seconds\n"; // prints the total time of execution

            string rows = $"Total number of valid rows: {number_of_valid_rows}, and Total number of invalid/skipped rows: {number_of_invalid_rows} \n"; // prints the number of valid and invalid rows
            

        

            WriteToLogFile(time_taken);
            WriteToLogFile(rows);


        



        }

        public class Person
        {
            // define Person class structure according to the data columns on the csv file
            private string firstName;

            [Name("First Name")]
            public string FirstName { get => firstName; set => firstName = value; }

            [Name("Last Name")]
            public string LastName { get; set; }
            [Name("Street Number")]
            public string StreetNumber { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string Province { get; set; }
            public string Country { get; set; }
            [Name("Phone Number")]
            public string PhoneNumber { get; set; }
            [Name("Postal Code")]
            public string PostalCode { get; set; }
            [Name("email Address")]
            public string EmailAddress { get; set; }
        }


        static void WriteToLogFile(string content)
        {
            string filePath = @"C:\Users\0B7744649\source\repos\ConsoleApp1\ConsoleApp1\Log\Log.txt"; // log file path

            try
            {
                //Append content to the log file
                File.AppendAllText(filePath, content);

            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access denied: {ex.Message}"); //check for exception
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Directory not found: {ex.Message}"); //check for exception
            }
            catch (IOException ex)
            {
                Console.WriteLine($"I/O error: {ex.Message}"); //check for exception
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}"); //check for exception
            }
        }

        static void ReadCsvFile(string filePath)
        {
            // create the date from the directory structure
            date = filePath.Replace(rootDirectory, "");
            string fileName = Path.GetFileName(filePath);
            date = date.Replace(fileName, "");
            date = date.Substring(1, date.Length - 2);
            date = date.Replace("\\", "/");
            number_of_rows = 0;
            // the info message for opening the file
            info = "INFO: Opening csv file " + filePath + "\n";
            WriteToLogFile(info);
            string error_message; // error message
            try
            {
                using (var reader = new StreamReader(filePath)) // reading the file
                using (StreamWriter writer = new StreamWriter(outputfilePath, true)) // writing to the output file
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    // Assuming the CSV has a header and the first row contains the field names
                    var records = csv.GetRecords<Person>();
                    List<string> invalidRecords = new List<string>();
                    string person_record;
                    
                    foreach (var record in records) // goes through every record on the csv file
                    {
                        if (!IsValid(record)) // checks if the record is valid
                        {
                            person_record=($"Invalid record: {record.FirstName}, {record.LastName}, {record.StreetNumber}, {record.Street}, {record.City}, {record.Province}, {record.Country}, {record.PostalCode}, {record.PhoneNumber}, {record.EmailAddress}\n");
                            invalidRecords.Add($"Invalid record: {record.FirstName}, {record.LastName}, {record.StreetNumber}, {record.Street}, {record.City}, {record.Province}, {record.Country}, {record.PostalCode}, {record.PhoneNumber}, {record.EmailAddress}");
                            number_of_invalid_rows++; // increases the number of invalid row count
                            number_of_rows++; // increases the row count in the csv
                            
                        }
                        else
                        {
                            person_record = $"{record.FirstName}, {record.LastName}, {record.StreetNumber}, {record.Street}, {record.City}, {record.Province}, {record.Country}, {record.PostalCode}, {record.PhoneNumber}, {record.EmailAddress}, {date}";
                            number_of_valid_rows++; // increases the number of valid row count
                            number_of_rows++; // increases the row count in the csv


                            writer.WriteLine(person_record); // write good data to the output file
                        }
                        
                    }
                }
            }
            catch (FileNotFoundException ex)  //check for exception
            {
                WriteToLogFile("File not found: " + ex.Message + "\n"); //write error message to log file
            }
            catch (UnauthorizedAccessException ex)  //check for exception
            {
                WriteToLogFile("Access denied: " + ex.Message + "\n"); //write error message to log file
            }
            catch (IOException ex)  //check for exception
            {
                WriteToLogFile("I/O error: " + ex.Message + "\n"); //write error message to log file
            }

            catch (Exception ex)  //check for exception
            {
                error_message = $"Error reading file {filePath}: {ex.Message}\n";  //write error message to log file
                number_of_invalid_rows++;
                number_of_rows++;
                WriteToLogFile(error_message);
            }

            info = "INFO: Closing csv file " + filePath + "\n"; // info to close csv file
            WriteToLogFile(info); // write info to log file
        }
        static void ListDirectories(string directory)
        {
            info = "INFO: Opening Directory " + directory +"\n"; // info to open directory
            WriteToLogFile(info); //write info to log file
            string error_message;
            try
            {
                // Get all subdirectories
                string[] subdirectories = Directory.GetDirectories(directory); // get the paths to the subdirectories

                foreach (string subdirectory in subdirectories)// go through each subdirectories
                {

                    ListDirectories(subdirectory); // recurively call the ListDirectories function 
                    
                    
                }

                string[] csvFiles = Directory.GetFiles(directory, "*.csv"); // get the csv files in the directories
                foreach (string csvFile in csvFiles)
                {
                    //Console.WriteLine(date);
                    ReadCsvFile(csvFile); // read each csv file
                }

                info = "INFO: Closing Directory " + directory + "\n"; // info for closing the directory
                WriteToLogFile(info); // write info to the log file


            }
            catch (UnauthorizedAccessException) // check for exceptions
            {
                error_message= $"Access denied to {directory}\n ";
                WriteToLogFile(error_message); //write error message to the log file
            }
            catch (DirectoryNotFoundException ex) // check for exceptions
            {
                error_message= $"Directory not found: {ex.Message} \n"; //write error message to the log file
                WriteToLogFile(error_message);
            }
            catch (PathTooLongException ex) // check for exceptions
            {
                error_message= $"Path is too long: {ex.Message} \n"; //write error message to the log file
                WriteToLogFile(error_message);
            }
            catch (IOException ex) // check for exceptions
            {
                error_message = $"I/O error: {ex.Message}\n"; //write error message to the log file
                WriteToLogFile(error_message);
            }
            catch (Exception ex) // check for exceptions
            {
                error_message = $"An error occurred: {ex.Message}\n"; //write error message to the log file
                WriteToLogFile(error_message);

            }
        }

        static bool IsValid(Person person)
        {
            //checking if each field contain NULL values, emppty string, special chracters 

            
            if (string.IsNullOrEmpty(person.FirstName) || containSpecialCharacter(person.FirstName)) {
                WriteToLogFile("First Name is NULL on row " + number_of_rows + ", therefore, logged as invalid/skipped row\n");
                return false;
            }
        
            if (string.IsNullOrEmpty(person.LastName) || containSpecialCharacter(person.LastName))
            {
                WriteToLogFile("Last Name is NULL on row " + number_of_rows +", therefore, logged as invalid/skipped row\n");
                return false;
            }
            if (string.IsNullOrEmpty(person.Country) || containSpecialCharacter(person.Country))
            {
                WriteToLogFile("Country is NULL on row " + number_of_rows + ", therefore, logged as invalid/skipped row\n");
                return false;
            }
            if (string.IsNullOrEmpty(person.City) || containSpecialCharacter(person.City))
            {
                WriteToLogFile("City is NULL on row " + number_of_rows + ", therefore, logged as invalid/skipped row\n");
                return false;
            }
            
            if (string.IsNullOrEmpty(person.Province) || containSpecialCharacter(person.Province))
            {
                WriteToLogFile("Province is NULL on row " + number_of_rows + ", therefore, logged as invalid/skipped row\n");
                return false;
            }
            
            if (string.IsNullOrEmpty(person.StreetNumber) || containSpecialCharacter(person.StreetNumber))
            {
                WriteToLogFile("Street Number is NULL on row " + number_of_rows + ", therefore, logged as invalid/skipped row\n");
                return false;
            }
            if (string.IsNullOrEmpty(person.Street) || containSpecialCharacter(person.Street))
            {
                WriteToLogFile("Street is NULL on row " + number_of_rows + ", therefore, logged as invalid/skipped row\n");
                return false;
            }

            if (string.IsNullOrEmpty(person.EmailAddress))
            {
                WriteToLogFile("Email Address is NULL on row " + number_of_rows + ", therefore, logged as invalid/skipped row\n");
                return false;
            }
            if (string.IsNullOrEmpty(person.PostalCode) || containSpecialCharacter(person.PostalCode))
            {
                WriteToLogFile("Postal Code is NULL on row " + number_of_rows +", therefore, logged as invalid/skipped row\n");
                return false;
            }
            if (string.IsNullOrEmpty(person.PhoneNumber) || containSpecialCharacter(person.PhoneNumber))
            {
                WriteToLogFile("Phone Number is NULL on row " + number_of_rows + ", therefore, logged as invalid/skipped row\n");
                return false;
            }
            // Validate Email
            if (!IsValidEmail(person.EmailAddress))
            {
                WriteToLogFile("Email Address is invalid on row " + number_of_rows + ", therefore, logged as invalid/skipped row\n");
                return false;
            }
               

            return true;
        }

        static bool IsValidEmail(string email)
        {
            // Simple regex for validating email format
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        static bool containSpecialCharacter(string content)
        {
            return content.Contains(",");
        }
    }
}
