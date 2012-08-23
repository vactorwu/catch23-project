//  .NET StockTrader Sample WCF Application for Benchmarking, Performance Analysis and Design Considerations for Service-Oriented Applications
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azurestocktrader.cloudapp.net
//                                   
//
//  Configuration Service 5 Notes:
//                      The application implements Configuration Service 5.0, for which the source code also ships in the sample. However, the .NET StockTrader 5
//                      sample is a general-purpose performance sample application for Windows Server and Windows Azure even if you are not implementing the Configuration Service. 
//                      
//

//======================================================================================================
// This is the Order Processor Service Custom Certificate validator.  It utilizes the provide base class,
// CustomCertificateValidator, that comes with Config Service.  This base class uses a list of thumbprints
// for valid certificates we want to accept for clients connecting with client certificates to the secured
// message security mode endpoint.  See the StockTrader Setup and Configuration Guide for details.  You
// must override the base method getAllowedThumbprints and provide your custom list, per below.  For OPS,
// the only two certs that will be allowed are those that ship with the StockTrader sample.  
//======================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using ConfigService.CustomValidators;

namespace Trade.OrderProcessorImplementation
{
    /// <summary>
    /// The Order Processor Service custom X.509 certificate validator, that uses the base class
    /// provided with Configuration Service.  This class is referenced in the config file, with the
    /// OPS_M_Security_Behavior behavior configuration for the host exe.
    /// </summary>
    public class CustomCertValidator : CustomCertificateValidator
    {
        /// <summary>
        /// Override to provide our list of valid cert thumbprints for the service.
        /// </summary>
        /// <returns></returns>
        protected override string[] getAllowedThumbprints()
        {
            List<string> thumbprints = new List<string>();
            //This is the thumbprint for the BSLClient Certificate in the StockTraderOPSClient.pfx file.  Spaces should be removed.
            thumbprints.Add("3a4f369cb3597d70512f5fdf786a9414f3746fb3");
            return thumbprints.ToArray();
        }
    }

    public class CustomUserNameValidator : ConfigService.CustomValidators.CustomUserNameValidator
    {
        
    }
}