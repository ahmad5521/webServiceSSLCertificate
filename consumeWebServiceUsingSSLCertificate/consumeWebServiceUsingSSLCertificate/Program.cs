using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using YakeenService;

namespace consumeWebServiceUsingSSLCertificate
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            #region Call Yakeen

            getEstablishmentInfoByMainIdResponse yakeenServiceResponse;
            getEstablishmentInfoByMainId yakeenServiceRequest = new getEstablishmentInfoByMainId();

            yakeenServiceRequest.EstablishmentInfoByMainIdRequest = new establishmentInfoByMainIdRequest();
            yakeenServiceRequest.EstablishmentInfoByMainIdRequest.clientIpAddress = "";
            yakeenServiceRequest.EstablishmentInfoByMainIdRequest.establishmentId = 00;
            yakeenServiceRequest.EstablishmentInfoByMainIdRequest.operatorId = 00;
            yakeenServiceRequest.EstablishmentInfoByMainIdRequest.password = "";
            yakeenServiceRequest.EstablishmentInfoByMainIdRequest.userName = "";
            yakeenServiceRequest.EstablishmentInfoByMainIdRequest.establishmentIdSpecified = false;
            yakeenServiceRequest.EstablishmentInfoByMainIdRequest.operatorIdSpecified = true;
            //yakeenServiceRequest.EstablishmentInfoByMainIdRequest.referenceNumber = Guid.NewGuid().ToString();
            yakeenServiceRequest.EstablishmentInfoByMainIdRequest.chargeCode = "";

            BasicHttpsBinding myBinding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
            myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            var myEndpoint = new EndpointAddress("URL");


            using (var myChannelFactory = new ChannelFactory<YakeenService.Yakeen4ELMX>(myBinding, myEndpoint))
            {
                myChannelFactory.Credentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication();
                myChannelFactory.Credentials.ServiceCertificate.SslCertificateAuthentication.CertificateValidationMode = X509CertificateValidationMode.None;
                //myChannelFactory.Credentials.ServiceCertificate.SslCertificateAuthentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
                //myChannelFactory.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new MyX509CertificateValidator("yakeen-piloting.eserve.com.sa");
                myChannelFactory.Credentials.ClientCertificate.SetCertificate(
                            StoreLocation.CurrentUser,
                            StoreName.My,
                            X509FindType.FindByIssuerName,
                            "certificate issuer name");
                try
                {
                    var client = myChannelFactory.CreateChannel();
                    yakeenServiceResponse = await client.getEstablishmentInfoByMainIdAsync(yakeenServiceRequest);
                    ((ICommunicationObject)client).Close();
                    myChannelFactory.Close();
                    //var s = base.Mapper.Map<establishmentInfoByMainIdResult, EstablishmentInfoByMainIdResponse>(yakeenServiceResponse.EstablishmentInfoByMainIdResult);
                    Console.WriteLine(yakeenServiceResponse.EstablishmentInfoByMainIdResult);
                }
                catch (Exception ex)
                {
                    myChannelFactory.Close();
                    //(client as ICommunicationObject)?.Abort();
                    Console.WriteLine(ex);
                }

            }

            #endregion
        }
    }
}
