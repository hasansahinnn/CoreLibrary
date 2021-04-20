using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application
{
    public static class Exceptions
    {
        public static void Message(string message) => throw new Exception(message);
        public static void Messages(List<string> message)
        {
            string errors = "";
            message.ForEach(x => errors += x + "/");
            throw new Exception(errors.Substring(0, errors.Length-1));
        }
        public static void UserNull() => throw new Exception("Kullanıcı Bulunamadı.Lütfen tekrar giriş yapın.");
        public static void DataNull() => throw new Exception("Kayıt Bulunamadı.");
        public static void PermissionNull() => throw new Exception("Bu Kayıta Erişmeye Yetkiniz Bulunmamaktadır.");
    }
}
