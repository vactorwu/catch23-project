using System;
using System.Text;
using System.IO;
using System.Net; 

namespace _365Library
{
    public class PostSubmitter
    {
        public string Send(string a_message, string a_MSISDN, string a_hubURL, string a_api_key, string a_keyword)
        {
            //Supply default values for optional arguments.
            string ackReplyAddress = "";    // Optional 
            string ackType = "";            // Optional 
            string operatorID = "";         // Optional 
            string encoding = "ASCII";      // set to ASCII or UCS2 
            string TPOA = "";               // Optional...originating address
            string subject = "";            // Optional - reference text for tracking purposes 

            string hubResponse = this.Send(a_message, a_MSISDN, a_hubURL, ackReplyAddress, ackType, operatorID, encoding, TPOA, subject, a_api_key, a_keyword);

            return hubResponse;
        }

        public string OptIn(string a_MSISDN, string a_hubURL, string a_api_key, string a_keyword)
        {
            String postBody;

            postBody = "Msisdn=" + a_MSISDN +
                "&Apikey=" + a_api_key +
                "&Keyword=" + a_keyword;
            string hubResponse = DoPost(a_hubURL, postBody);

            return hubResponse;
        }

        public string OptOut(string a_MSISDN, string a_hubURL, string a_api_key, string a_keyword)
        {
            String postBody;

            postBody = "Msisdn=" + a_MSISDN +
                "&Apikey=" + a_api_key +
                "&Keyword=" + a_keyword;
            string hubResponse = DoPost(a_hubURL, postBody);

            return hubResponse;
        }

        public string OptInShow(string a_hubURL, string a_api_key, string a_keyword)
        {
            String postBody;

            postBody = "Apikey=" + a_api_key +
                "&Keyword=" + a_keyword;
            string hubResponse = DoPost(a_hubURL, postBody);

            return hubResponse;
        }

        public string OptOutShow(string a_hubURL, string a_api_key, string a_keyword)
        {
            String postBody;

            postBody = "Apikey=" + a_api_key +
                "&Keyword=" + a_keyword;
            string hubResponse = DoPost(a_hubURL, postBody);

            return hubResponse;
        }

        public string RetrieveLog(string a_hubURL, string a_api_key, string a_keyword, string a_log, string a_msisdn, string a_startdate)
        {
            String postBody;

            postBody = "Apikey=" + a_api_key +
                "&Log=" + a_log +
            "&Keyword=" + a_keyword +
            "&Msisdn=" + a_msisdn +
            "&Date=" + a_startdate;
            string hubResponse = DoPost(a_hubURL, postBody);

            return hubResponse;
        }

        public string RetrieveLog(string a_hubURL, string a_api_key, string a_keyword, string a_log )
        {
            String postBody;

            postBody = "Apikey=" + a_api_key +
                "&Log=" + a_log +
            "&Keyword=" + a_keyword;
            string hubResponse = DoPost(a_hubURL, postBody);

            return hubResponse;
        }

        public string Send(string a_message, string a_MSISDN, string a_hubURL, string a_ackReplyAddress, string a_ackType, string a_operatorID, string a_encoding, string a_TPOA, string a_subject, string a_api_key, string a_keyword)
        {
            string hubURL = a_hubURL;                   // Required 
            string message = a_message;                 // Required 
            string MSISDN = a_MSISDN;                   // Required - for multiple numbers use a comma seperated list 
            string ackReplyAddress = a_ackReplyAddress; // Optional 
            string ackType = a_ackType;                 // Optional 
            string operatorID = a_operatorID;           // Optional 
            string encoding = a_encoding;               // set to ASCII or UCS2 
            string TPOA = a_TPOA;                       // Optional...originating address
            string subject = a_subject;                 // Optional - reference text for tracking purposes 

            string postBody = "";
            string messageLength = "0";

            if (encoding == "UCS2")
            {
                int length = message.Length / 4;
                messageLength = length.ToString();
            }

            string setup = "";

            if (ackReplyAddress != "")
            {
                // Get the acknowledgement type that was set (probably equal to MESSAGE) 
                setup = setup + "\r\nAckType=MESSAGE";

                // Now tell the hub where to send the acknowledgements to on this server 
                setup = setup + "\r\nAckReplyAddress=" + ackReplyAddress;

                if (ackType == "MOBILE")
                {
                    // If mobile notification has been set, include this 
                    setup = setup + "\r\nMobileNotification=YES";
                }
            }

            // set the carriage returns 
            message = message.Replace("\r\n", "<CR>");
            message = message.Replace("\n", "<CR>");
            message = message.Replace("\r", "<CR>");

            // If it's set, insert CARRIER information here (usually only set for HK numbers) 
            if (operatorID != "")
            {
                setup = setup + "\r\nOperatorId=" + operatorID;
            }

            // This is if it's a western message (i.e plain ascii or someone just forgot to set it)                         
            if (encoding == "ASCII")
            {
                // Only set TPOA if it actually has been set by the programmer 
                if (TPOA == "")
                {
                    //postBody = "Subject=" + subject +
                    //    "\r\n[MSISDN]\r\nList=" + MSISDN +
                    //    "\r\n[MESSAGE]\r\nText=" + message +
                    //    "&License=" + a_api_key;

                    postBody = "Subject=" + subject +
                    "\r\n[MSISDN]\r\nList=" + MSISDN +
                    "\r\n[MESSAGE]\r\nText=" + message +
                    "&Apikey=" + a_api_key+
                    "&Keyword=" + a_keyword;

                    if (!setup.Equals(""))
                    {
                        postBody = postBody + "\r\n[Setup]" + setup;
                    }
                }
                else
                    postBody = "Subject=" + subject +
                        "\r\n[MSISDN]\r\nList=" + MSISDN +
                        "\r\n[MESSAGE]\r\nText=" + message +
                        "\r\n[Setup]\r\nOriginatingAddr=" + TPOA + setup;
            }
            else if (encoding == "UCS2")
            {
                // Only set TPOA if it actually has been set by the programmer 
                if (TPOA == "")
                    postBody = "Subject=" + subject +
                        "\r\n[MSISDN]\r\nList=" + MSISDN +
                        "\r\n[MESSAGE]\r\nBinary=" + message +
                        "\r\nLength=" + messageLength +
                        "\r\n[Setup]\r\nDCS=" + encoding + setup;
                else
                    postBody = "Subject=" + subject +
                        "\r\n[MSISDN]\r\nList=" + MSISDN +
                        "\r\n[MESSAGE]\r\nBinary=" + message +
                        "\r\nLength=" + messageLength +
                        "\r\n[Setup]\r\nDCS=" + encoding +
                        "\r\nOriginatingAddr=" + TPOA + setup;
            }

            string hubResponse = DoPost(hubURL, postBody);

            return hubResponse;
        }

        public string DoPost(string hubURL, string postBody)
        {
            WebResponse result = null;
            Stream receiveStream = null;
            StreamReader streamReader = null;
            string hubResponse = "";

            try
            {
                // Create Web Request and HTTP appropriate headers 
                WebRequest req = WebRequest.Create(hubURL);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = postBody.Length;

                byte[] SomeBytes = null;

                Encoding encode1 = Encoding.GetEncoding("iso-8859-15");

                // if a euro sign is in the message replace it with "ÿe" so the hub can recognise it 
                postBody = postBody.Replace("€", "ÿe");
                SomeBytes = encode1.GetBytes(postBody);
                req.ContentLength = SomeBytes.Length;
                Stream newStream = req.GetRequestStream();
                newStream.Write(SomeBytes, 0, SomeBytes.Length);
                newStream.Close();

                // our stream to read the HTTP Response 
                result = req.GetResponse();
                receiveStream = result.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("iso-8859-15");
                streamReader = new StreamReader(receiveStream, encode);
                Char[] read = new Char[256];
                int count = streamReader.Read(read, 0, 256);


                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    hubResponse = hubResponse + str;
                    count = streamReader.Read(read, 0, 256);
                }
            }
            finally
            {
                if (streamReader != null)
                    streamReader.Close();
                if (receiveStream != null)
                    receiveStream.Close();
                if (result != null)
                    result.Close();
            }
            return hubResponse;
        } 

    }
}
