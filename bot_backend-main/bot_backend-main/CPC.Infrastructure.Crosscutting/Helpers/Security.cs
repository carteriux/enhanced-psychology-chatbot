using CPC.Domain.DTO.ValueObjects;
using CPC.Infrastructure.CrossCutting.ICommon;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CPC.Infrastructure.CrossCutting.Helpers
{
    /// <summary>
    /// Contiene metodos para la seguridad.
    /// </summary>
    public class Security : ISecurity
    {

        /// <summary>
        /// Crea el Jason Web Token para el Login del usuario.
        /// </summary>
        /// <param name="sesion">variable de tipo LocalSession</param>
        /// <param name="key">Recibe llave tipo string</param>
        /// <returns>Retorna Token Encriptado</returns>
        public string CreateJwtToken(LocalSession sesion, string key)
        {            
            //var securityKey = System.Text.Encoding.UTF8.GetBytes(key);

            var ListaClains = new List<Claim>
            {
                new Claim("IdUser", sesion.IdUser.ToString()),
                new Claim("Email", sesion.Email),
                new Claim("EnrollmentNumber", sesion.EnrollmentNumber.ToString()),                
            };

            //if (sesion.Session != null)
            //{
            //    ListaClains.Add(new Claim("sesionId", sesion.Session.SessionId));
            //    ListaClains.Add(new Claim("IssuedOn", sesion.Session.IssuedOn.ToString()));
            //    ListaClains.Add(new Claim("ExpiresOn", sesion.Session.ExpiresOn.ToString()));
            //    ListaClains.Add(new Claim("IP", sesion.Session.IP));
            //}

            var claimsIdentity = new ClaimsIdentity(ListaClains, "Custom");                       

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            
            var jwtConfig = new JwtSecurityToken(
                claims: ListaClains,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        } 

        /// <summary>
        /// Desencripta el token y lo convierte a un DTO.
        /// </summary>
        /// <param name="Token">Recibe Token encriptado</param>
        /// <returns>Retorna Token convertido en DTO</returns>
        public LocalSession DecodingJwtToken(string token)
        {
            var jsonToken = new JwtSecurityToken();
            var TokenDes = token.Decrypt();
            
            //var sesionDTO = new SessionDTO();
            var LocalSesi = new LocalSession();
            
            var stream = TokenDes;

            var handler = new JwtSecurityTokenHandler();

            try
            {
                jsonToken = handler.ReadToken(stream) as JwtSecurityToken;
            }
            catch (Exception)
            {
                return new LocalSession();
            }            


            var EnrollmentNumber = (from N in jsonToken.Claims
                                    where N.Type.Trim() == "EnrollmentNumber"
                                    select N.Value).FirstOrDefault();

            var IdUser = (from N in jsonToken.Claims
                          where N.Type.Trim() == "IdUser"
                          select N.Value).FirstOrDefault();

            var Email = (from N in jsonToken.Claims
                         where N.Type.Trim() == "Email"
                         select N.Value).FirstOrDefault();

            var FechaUTC = (from N in jsonToken.Claims
                            where N.Type.Trim() == "FechaUTC"
                            select N.Value).FirstOrDefault();

            //var sesionId = (from N in jsonToken.Claims
            //           where N.Type.Trim() == "sesionId"
            //           select N.Value).FirstOrDefault();

            //var IssuedOn = (from N in jsonToken.Claims
            //           where N.Type.Trim() == "IssuedOn"
            //           select N.Value).FirstOrDefault();

            //var ExpiresOn = (from N in jsonToken.Claims
            //           where N.Type.Trim() == "ExpiresOn"
            //           select N.Value).FirstOrDefault();

            //var IP = (from N in jsonToken.Claims
            //           where N.Type.Trim() == "IP"
            //           select N.Value).FirstOrDefault();

            //sesionDTO.SessionId = sesionId;
            //sesionDTO.UserId = IdUser;
            //sesionDTO.IssuedOn = DateTime.Parse(IssuedOn);
            //sesionDTO.ExpiresOn = DateTime.Parse(ExpiresOn);
            //sesionDTO.IP = IP;


            //LocalSesi.Session = sesionDTO;
            LocalSesi.Email = Email;
            LocalSesi.EnrollmentNumber = EnrollmentNumber;
            LocalSesi.FechaUTC = DateTime.Parse(FechaUTC);            

            return LocalSesi;
        }

        public bool Validate(string appiKey, string dateSent)
        {
            bool value = false;
            try
            {
                DateTime time = Convert.ToDateTime(dateSent);
                DateTime utc = DateTime.UtcNow;
                
                int mm = int.Parse(ConfigurationManager.AppSettings["Minutos"]);

                TimeSpan duration = new TimeSpan(0, 0, mm, 0);                
                DateTime answer = time.Add(duration);                

                int nHoraActual = Convert.ToInt32(utc.ToString("hhmmss"));
                int nHoraFinal = Convert.ToInt32(answer.ToString("hhmmss"));                

                if (nHoraActual <= nHoraFinal)
                {
                    string x = ConfigurationManager.AppSettings["KeyToken"];                                        

                    if (appiKey.ToUpper() == x.ToUpper())
                    {
                        value = true;
                    }

                }
            }
            catch (Exception)
            {
                value = false;
            }
            return value;
        }

        public static string Sign(string str, string key)
        {
            var encoding = new ASCIIEncoding();

            byte[] signature;

            using (var crypto = new HMACSHA256(encoding.GetBytes(key)))
            {
                signature = crypto.ComputeHash(encoding.GetBytes(str));
            }

            return Base64Encode(signature);
        }

        public static string Base64Encode(dynamic obj)
        {
            Type strType = obj.GetType();

            var base64EncodedValue = Convert.ToBase64String(strType.Name.ToLower() == "string" ? Encoding.UTF8.GetBytes(obj) : obj);

            return base64EncodedValue;
        }

        public static dynamic Base64Decode(string str)
        {
            var base64DecodedValue = Convert.FromBase64String(str);

            return base64DecodedValue;
        }

        public List<Claim> GetClainsList<T>(T Dto) where T : class, new()
        {
            var Lista = new List<Claim>();

            try
            {                
                object result = null;

                Lista.AddRange(from p in Dto.GetType().GetProperties()
                               where (p.PropertyType.BaseType == typeof(ValueType)
                               || p.PropertyType == typeof(string))
                               select new Claim(p.Name, ((result = p.GetValue(Dto, null)) != null ? result.ToString() : string.Empty), p.PropertyType.FullName));

                var classProp = (from p in Dto.GetType().GetProperties()
                                 where (p.PropertyType.IsClass
                                 && p.PropertyType != typeof(string)
                                 && !p.PropertyType.IsGenericType)
                                 select new
                                 {
                                     Properties = p.PropertyType.GetProperties(),
                                     Value = p.GetValue(Dto, null)
                                 }).ToList();               


                Lista.AddRange(from it in classProp
                               let ps = classProp.SelectMany(cp => cp.Properties)
                               from p in ps
                               select new Claim(p.Name, ((result = p.GetValue(it.Value, null)) != null ? result.ToString() : string.Empty), p.PropertyType.FullName));

                var genericProp = (from p in Dto.GetType().GetProperties()
                                   where p.PropertyType.IsGenericType
                                   && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>)
                                   select new
                                   {
                                       Properties = p.PropertyType.GetGenericArguments()[0].GetType().GetProperties(),
                                       Value = p.GetValue(Dto, null)
                                   }).ToList();

                Lista.AddRange(from g in genericProp
                               let cs = classProp.SelectMany(cp => cp.Properties)
                               from c in cs
                               select new Claim(c.Name, ((result = c.GetValue(g.Value, null)) != null ? result.ToString() : string.Empty), c.PropertyType.FullName));
               
                //else if (entityProperty.PropertyType.IsGenericType && entityProperty.PropertyType.GetGenericTypeDefinition() == typeof(List<>))                
                //PropertyInfo[] properties2 = entityProperty.PropertyType.GetGenericArguments()[0].GetType().GetProperties();
                
            }
            catch(Exception Ex)
            {

            }


            return Lista;
        }

    }
}
