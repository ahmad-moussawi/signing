using System;
using System.Linq;
using Org.BouncyCastle.Cms;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Digests;
using System.Text;

namespace Signing
{
    public class Toolkit
    {
        protected readonly Util.String stringUtil;

        public Toolkit()
        {
            stringUtil = new Util.String();
        }

        /// <summary>
        /// Generate an Attached Signature, the file content will be embedded in the signature without encryption
        /// </summary>
        /// <param name="signatures">The list of detached PCKS7 signatures</param>
        /// <param name="payload">The clear text file content</param>
        /// <returns>Attached PCKS7 signature</returns>
        public byte[] Sign(List<byte[]> signatures, byte[] payload)
        {
            CmsSignedDataGenerator generator = new CmsSignedDataGenerator();

            var signersInfos = signatures
                .Select(x => new CmsSignedDataParser(x))
                .Select(x => x.GetSignerInfos());

            foreach (var item in signersInfos)
            {
                generator.AddSigners(item);
            }

            var signed = generator.Generate(CmsSignedGenerator.Data, new CmsProcessableByteArray(payload), true);

            return signed.ContentInfo.GetDerEncoded();
        }


        /// <summary>
        /// Convert the Attached Signature (Signed File) to Base64 format
        /// </summary>
        /// <param name="bytes">The PCKS7 Attached Signature (Signed File)</param>
        /// <param name="withHeaderAndFooter">Whether to include the "-----BEGIN PKCS7-----" and "-----END PKCS7-----" header and footer</param>
        /// <returns>The Base64 format of the Attached Signature</returns>            
        public string ToBase64Format(byte[] bytes, bool withHeaderAndFooter = true)
        {
            var chunks = stringUtil.Chunk(Convert.ToBase64String(bytes), 64).ToList();

            if (withHeaderAndFooter)
            {
                chunks.Insert(0, "-----BEGIN PKCS7-----");
                chunks.Add("-----END PKCS7-----");
            }

            return string.Join(Environment.NewLine, chunks);
        }

        /// <summary>
        /// Generate a SHA1-256 Digest
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string Disgest(byte[] bytes)
        {
            Sha256Digest engine = new Sha256Digest();
            engine.BlockUpdate(bytes, 0, bytes.Length);
            byte[] hash = new byte[engine.GetDigestSize()];
            engine.DoFinal(hash, 0);
            engine.Finish();
            engine.Reset();
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Generate a SHA1-256 Digest
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string Digest(string content)
        {
            return Disgest(Encoding.UTF8.GetBytes(content));
        }

    }
}
