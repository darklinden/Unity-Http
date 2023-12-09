using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

using UnityEngine.Networking;

namespace Http
{
    public class SSL_Handler : CertificateHandler
    {
        // Based on https://www.owasp.org/index.php/Certificate_and_Public_Key_Pinning#.Net
        // AcceptAllCertificatesSignedWithASpecificPublicKey
        // Encoded RSAPublicKey
        // private static string PUB_KEY = "somepublickey";

        protected override bool ValidateCertificate(byte[] certificateData)
        {
            // https://stackoverflow.com/a/76679466
            // you would actually need to use the normal system validation and e.g. go through X509Chain

            // var certificate = new X509Certificate2(certificateData);
            // var chain = new X509Chain();
            // chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            // chain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
            // chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;

            // if (chain.Build(certificate))
            // {
            //     return true;
            // }

            // check the public key

            // X509Certificate2 certificate = new X509Certificate2(certificateData);
            // string pk = certificate.GetPublicKeyString();
            // Debug.Log(pk.ToLower());
            // return pk.Equals(PUB_KEY);

            Log.D("SSL_Handler.ValidateCertificate", certificateData);
            return true;
        }

        // CertificateHandler will be released after WebRequest Disposed.
        // So we NEED to Create a new one every time.
        public static SSL_Handler Default
        {
            get
            {
                return new SSL_Handler();
            }
        }
    }
}