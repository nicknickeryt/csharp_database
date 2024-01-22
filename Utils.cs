namespace SimpleDatabase
{

    /* Utils
     *
     * This class contains some handy utils for SimpleDatabase.
     *
     * These include:
     *  debug mode
     *  printing spacers
     *  error & datatype handlers
     *  Console.Write & Console.WriteLine wrappers for code clarity
     *
     */

    class Utils
    {

        public static bool isDebugEnabled = false;

        public static void printSpacer()
        {
            printf("──────────────────────────────────────────────────────────");
        }


        public enum ErrorType
        {
            HEADER,
            AMOUNT,
            TYPE,
            SIZE,
            CSV,
            JSON,
            INTENTATIONAL
        }

        private static Dictionary<ErrorType, string> ErrorMessages = new Dictionary<ErrorType, string>(){
            { ErrorType.HEADER, "[Error]: Header not found."},
            { ErrorType.AMOUNT, "[Error] Wrong amount of data."},
            { ErrorType.TYPE, "[Error]: Wrong data type."},
            { ErrorType.SIZE, "[Error]: Wrong data size."},
            { ErrorType.CSV, "[Error]: Error while exporting CSV file."},
            { ErrorType.JSON, "[Error]: Error while serializing table/database."},
            { ErrorType.INTENTATIONAL, "[Note]: the error below is intentational for debugging purposes."}
        };

        public static string getErrMsg(ErrorType errorType)
        {
            return ErrorMessages[errorType];
        }

        public static void printDebugError(ErrorType errorType)
        {
            printDebug(getErrMsg(errorType));
        }
        public static void printError(ErrorType errorType)
        {
            printf(getErrMsg(errorType));
        }

        public static DataType getDataType(object o)
        {
            if (o is int)
            {
                return DataType.INT;
            }
            else if (o is float)
            {
                return DataType.FLOAT;
            }
            else if (o is double)
            {
                return DataType.DOUBLE;
            }
            else if (o is string)
            {
                return DataType.STRING;
            }
            else if (o is bool)
            {
                return DataType.BOOLEAN;
            }
            else
            {
                printDebugError(ErrorType.TYPE);
                printDebug("Supported DataTypes:");
                foreach (string dataType in Enum.GetNames(typeof(DataType)))
                {
                    printf(dataType);

                }
            }
            return DataType.UNKNOWN;
        }

        public static bool checkDataType(Header header, object value)
        {
            if (getDataType(value) == header.DataType) return true;
            printDebugError(ErrorType.TYPE);
            return false;
        }

        public static void printDebug(string str)
        {
            if (isDebugEnabled) printf("Debug: " + str);
        }
        public static void print(string str)
        {
            Console.Write(str);
        }
        public static void printf(string str)
        {
            Console.WriteLine(str);
        }

    }
}