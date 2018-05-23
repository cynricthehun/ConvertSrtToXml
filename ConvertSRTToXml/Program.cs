using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConvertSRTToXml
{
    class Program
    {

        static void Main(string[] args)
        {
            string file;
            bool still = false;
            while (still == false)
            {
                Console.WriteLine("Welcome to the SRT File converter.");
                Console.WriteLine("Please enter the file name and path to begin the process.");
                string newFile = selectFile();
                readFile(newFile);
                Console.WriteLine("type 'y' to quit.");
                var finished = Console.ReadLine();
                if (finished == "y")
                {
                    still = true;
                }
            }

            string selectFile()
            {
                file = Console.ReadLine();
                return file;
            }

            string getFileName(string theFile)
            {
                string fileName = Path.GetFileNameWithoutExtension(theFile);
                return fileName;
            }

            string getExecutingDirectory()
            {
                string DirectoryString = Directory.GetCurrentDirectory();
                return DirectoryString;
            }

            string setupHeader()
            {
                string header =
                    "﻿<?xml version='1.0' encoding='UTF - 8'?>\n" +
                    "<tt xmlns='http://www.w3.org/2006/04/ttaf1' xmlns:tts='http://www.w3.org/2006/04/ttaf1#styling' xml:lang='en'> \n" +
                    "<head> \n" +
                    "<styling> \n" +
                    "<style id='defaultCaption' tts:fontSize='18' tts:fontFamily='Trebuchet MS, Arial, SansSerif' tts:fontWeight='normal' tts:fontStyle='normal' tts:textDecoration='none' tts:color='white' tts:backgroundColor='black' tts:opacity='0.35' tts:textAlign='center'/> \n" +
                    "</styling> \n" +
                    "</head> \n" +
                    "<body style='defaultCaption' id='thebody'> \n" +
                    "<div xml:lang='eng'> \n";
                return header;
            }

            string setupFooter()
            {
                string footer =
                    "</div> \n" +
                    "</body> \n" +
                    "</tt> \n";
                return footer;
            }

            int randomNumber()
            {
                Random rnd = new Random();
                int number = rnd.Next(100000, 2000000);
                return number;
            }

            void readFile(string newbie)
            {
                string[] lines = System.IO.File.ReadAllLines(@newbie);
                string captionNumber = "";
                string oldCaptionNumber = "1";
                string startTime = "00:00:00.0";
                string endTime = "00:00:00.0";
                string content = "";
                string header1 = setupHeader();
                string footer1 = setupFooter();

                char oldChar = ',';
                char newChar = '.';

                List<string> newLines = new List<string>();

                // Add Header to new array;
                newLines.Add(header1);
                Console.WriteLine(header1);

                //Print out the paragraph tags with their content and values.
                foreach (string line in lines)
                {
                    if (line.Length > 0 && char.IsDigit(line[0]) && line[0].ToString() != "0")
                    {
                        if (line.Length > 1)
                        {
                            captionNumber = line[0].ToString() + line[1].ToString();
                        } else
                        {
                            captionNumber = line[0].ToString();
                        }
                    }
                    // Format Start and End Times
                    else if (line.Length > 0 && line[0].ToString() == "0")
                    {
                        string allCharacterLeftOfDash = line.Substring(0, line.IndexOf("-"));
                        startTime = "'" + allCharacterLeftOfDash.Trim() + "'";
                        // Replace the , with a .
                        startTime = startTime.Replace(oldChar, newChar);
                        string allCharacterRightOfArrow = line.Split('>').Last();
                        endTime = "'" + allCharacterRightOfArrow.Trim() + "'";
                        // Replace the , with a .
                        endTime = endTime.Replace(oldChar, newChar);
                    }
                    else if (line.Length == 0)
                    {
                        // Blank line.
                    } else
                    {
                        // Catch all lines that aren't accounted for.
                    }

                    if (line.Length > 0 && char.IsLetter(line[0]) && line[0].ToString() != "0")
                    {
                        if (oldCaptionNumber == captionNumber)
                        {
                            content += line + " ";
                        }
                    }

                    if (oldCaptionNumber != captionNumber)
                    {
                        Console.WriteLine("\t" + "<p begin='{0}' end='{1}'>{2}</p>", startTime, endTime, content);
                        string newPara = "\t <p begin=" + startTime + " end=" + endTime + ">" + content + "</p>";
                        // Add line to new array;
                        newLines.Add(newPara);
                        content = "";
                    }

                    if (line.Length > 0 && char.IsDigit(line[0]) && line[0].ToString() != "0")
                    {
                        if (captionNumber != oldCaptionNumber)
                        {
                            oldCaptionNumber = captionNumber;
                        }
                    }
                }

                //Add footer to new array
                newLines.Add(footer1);
                Console.WriteLine(footer1);

                //Get Rando
                int rando = randomNumber();

                //Get the current directory
                string directoryForPath = getExecutingDirectory();

                //GetFileName
                string fileNameForPath = getFileName(file);

                //Write collection to new file.
                System.IO.File.WriteAllLines(@directoryForPath + "/" + fileNameForPath + "" + DateTime.Now.Millisecond + ".xml", newLines);
            }
        }

    }
}
