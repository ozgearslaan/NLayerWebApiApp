using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs
{
    public class CustomResponseDto<T>
    {//generic data T
        public T Data { get; set; }
        //geriye bir data dönmek istemediğimizde nocontentdto oluşturduk
        [JsonIgnore]
        public int StatusCode { get; set; }
        //endpointe istek yaptığımızda durum kodu alırız, ama status kodu clientlara dönmeye gerek yok.
        public List<string> Errors { get; set; }
        //new ile oluşturmak yerine staticle oluşturduk
        //success olunca dön
        public static CustomResponseDto<T> Success(int statusCode, T data)
        {
            return new CustomResponseDto<T> { Data = data, StatusCode = statusCode, Errors = null };
        }
        //productupdate de datayı dönmeye gerek yok
        public static CustomResponseDto<T> Success(int statusCode)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode};
        }
        public static CustomResponseDto<T> Fail(int statusCode, List<string> errors)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = errors };
        }//geriye instance döndüm new ile yeni üretmek yerine
        //static factory design pattern

        public static CustomResponseDto<T> Fail(int statusCode, string error)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = new List<string> { error } };
        }
        //birden fazla error dönebilirim
    }
}
