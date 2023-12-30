using System.Diagnostics;

namespace BazaDanych
{
    class Utils
    {
        
        public static bool isDebugEnabled = false;

        public static void printSpacer()
        {
            Console.WriteLine("> ----------------------------------------");
        }

        public static DataType getDataType(Object o){
            if(o is int){
                return DataType.INT;
            }
            if(o is string){
                return DataType.STRING;
            }
            else {
                printDebug("data type is undefined ");
            }
            return DataType.UNKNOWN;
        }   

        public static void printDebug(string str) {
            if(isDebugEnabled) Console.WriteLine("Debug: " + str);
        }

    }
}