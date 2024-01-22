using System.Collections;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace BazaDanych
{
    class Utils
    {
        
        public static bool isDebugEnabled = false;

        public static void printSpacer()
        {
            Console.WriteLine("> ----------------------------------------");
        }


        public enum ErrorType {
            HEADER_NOT_FOUND,
            WRONG_AMOUNT_DATA,
            WRONG_DATA_TYPE
        }

        private static Dictionary<ErrorType, string> ErrorMessages = new Dictionary<ErrorType, string>(){
            { ErrorType.HEADER_NOT_FOUND, "Header not found."},
            {ErrorType.WRONG_AMOUNT_DATA, "Wrong amount of data."},
            {ErrorType.WRONG_DATA_TYPE, "Wrong data type."}
        };

        public static string getErrMsg(ErrorType errorType){
            return ErrorMessages[errorType];
        }

        public static DataType getDataType(Object o){
            if(o is int){
                return DataType.INT;
            }
            else if(o is float){
                return DataType.FLOAT;
            }
            else if(o is double){
                return DataType.DOUBLE;
            }
            else if(o is string){
                return DataType.STRING;
            }
            else if(o is bool){
                return DataType.BOOLEAN;
            }
            else {
                printDebug("DataType not supported. Supported DataTypes:");
                foreach (string dataType in Enum.GetNames(typeof(DataType))){
                    printLine(dataType);

                }
            }
            return DataType.UNKNOWN;
        }   

        public static bool checkDataType(Header header, object value){
            if(getDataType(value) == header.DataType) return true;
            printDebug("Wrong data type!");
            return false;
        }

        public static void printDebug(string str) {
            if(isDebugEnabled) Console.WriteLine("Debug: " + str);
        }
        public static void print(string str) {
            Console.Write(str);
        }
        public static void printLine(string str) {
            Console.WriteLine(str);
        }

    }
}