using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace IotApi.Core.Utils
{
    /// <summary>
    /// SM2加密工具类（采用十六进制格式）
    /// 注意：此类需要添加BouncyCastle库依赖
    /// NuGet包：Portable.BouncyCastle
    /// </summary>
    public class SM2Utils
    {
        /// <summary>
        /// 公钥常量
        /// </summary>
        public const string KEY_PUBLIC_KEY = "publicKey";

        /// <summary>
        /// 私钥返回值常量
        /// </summary>
        public const string KEY_PRIVATE_KEY = "privateKey";

        /// <summary>
        /// SM2加密算法
        /// </summary>
        /// <param name="publicKey">十六进制公钥</param>
        /// <param name="data">明文数据</param>
        /// <returns>十六进制密文</returns>
        public static string Encrypt(string publicKey, string data)
        {
            // 注意：此方法需要添加BouncyCastle库实现
            // 请添加NuGet包：Portable.BouncyCastle
            // 实现示例:
            /*
            try
            {
                // 获取一条SM2曲线参数
                X9ECParameters sm2ECParameters = GMNamedCurves.GetByName("sm2p256v1");
                // 构造ECC算法参数，曲线方程、椭圆曲线G点、大整数N
                ECDomainParameters domainParameters = new ECDomainParameters(sm2ECParameters.Curve, sm2ECParameters.G, sm2ECParameters.N);
                //提取公钥点
                ECPoint pukPoint = sm2ECParameters.Curve.DecodePoint(Hex.Decode(publicKey));
                // 公钥前面的02或者03表示是压缩公钥，04表示未压缩公钥, 04的时候，可以去掉前面的04
                ECPublicKeyParameters publicKeyParameters = new ECPublicKeyParameters(pukPoint, domainParameters);

                SM2Engine sm2Engine = new SM2Engine(SM2Engine.Mode.C1C3C2);
                // 设置sm2为加密模式
                sm2Engine.Init(true, new ParametersWithRandom(publicKeyParameters, new SecureRandom()));

                byte[] inputData = Encoding.UTF8.GetBytes(data);
                byte[] arrayOfBytes = sm2Engine.ProcessBlock(inputData, 0, inputData.Length);
                return Hex.ToHexString(arrayOfBytes);
            }
            catch (Exception e)
            {
                throw new Exception("SM2加密失败", e);
            }
            */
            
            throw new NotImplementedException("请添加BouncyCastle库实现SM2加密");
        }

        /// <summary>
        /// SM2解密算法
        /// </summary>
        /// <param name="privateKey">十六进制私钥</param>
        /// <param name="cipherData">十六进制密文数据</param>
        /// <returns>明文</returns>
        public static string Decrypt(string privateKey, string cipherData)
        {
            // 注意：此方法需要添加BouncyCastle库实现
            // 请添加NuGet包：Portable.BouncyCastle
            // 实现示例:
            /*
            try
            {
                // 使用BC库加解密时密文以04开头，传入的密文前面没有04则补上
                if (!cipherData.StartsWith("04"))
                {
                    cipherData = "04" + cipherData;
                }
                byte[] cipherDataByte = Hex.Decode(cipherData);
                BigInteger privateKeyD = new BigInteger(privateKey, 16);
                //获取一条SM2曲线参数
                X9ECParameters sm2ECParameters = GMNamedCurves.GetByName("sm2p256v1");
                //构造domain参数
                ECDomainParameters domainParameters = new ECDomainParameters(sm2ECParameters.Curve, sm2ECParameters.G, sm2ECParameters.N);
                ECPrivateKeyParameters privateKeyParameters = new ECPrivateKeyParameters(privateKeyD, domainParameters);

                SM2Engine sm2Engine = new SM2Engine(SM2Engine.Mode.C1C3C2);
                // 设置sm2为解密模式
                sm2Engine.Init(false, privateKeyParameters);

                byte[] arrayOfBytes = sm2Engine.ProcessBlock(cipherDataByte, 0, cipherDataByte.Length);
                return Encoding.UTF8.GetString(arrayOfBytes);
            }
            catch (Exception e)
            {
                throw new Exception("SM2解密失败", e);
            }
            */
            
            throw new NotImplementedException("请添加BouncyCastle库实现SM2解密");
        }

        /// <summary>
        /// 生成密钥对
        /// </summary>
        /// <returns>包含公钥和私钥的字典</returns>
        public static Dictionary<string, string> CreateKey()
        {
            // 注意：此方法需要添加BouncyCastle库实现
            // 请添加NuGet包：Portable.BouncyCastle
            // 实现示例:
            /*
            try
            {
                ECGenParameterSpec sm2Spec = new ECGenParameterSpec("sm2p256v1");
                // 获取一个椭圆曲线类型的密钥对生成器
                KeyPairGenerator kpg = KeyPairGenerator.GetInstance("EC", new BouncyCastleProvider());
                // 使用SM2参数初始化生成器
                kpg.Initialize(sm2Spec);
                // 获取密钥对
                KeyPair keyPair = kpg.GenerateKeyPair();
                PublicKey publicKey = keyPair.Public;
                BCECPublicKey p = (BCECPublicKey)publicKey;
                PrivateKey privateKey = keyPair.Private;
                BCECPrivateKey s = (BCECPrivateKey)privateKey;
                
                Dictionary<string, string> result = new Dictionary<string, string>();
                result.Add(KEY_PUBLIC_KEY, Hex.ToHexString(p.Q.GetEncoded(false)));
                result.Add(KEY_PRIVATE_KEY, Hex.ToHexString(s.D.ToByteArray()));
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("生成SM2密钥对失败", e);
            }
            */
            
            throw new NotImplementedException("请添加BouncyCastle库实现SM2密钥对生成");
        }
    }
}