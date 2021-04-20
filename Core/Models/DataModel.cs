using Business.ValidationRules.FluentValidation;
using Core.Application;
using Core.Application.Interfaces;
using Core.Caching;
using Core.DataSecurity;
using Core.Security;
using Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class DataModel : BaseEntity, IDataModel, IAggregateRoot
    {
        public int ModelId { get; set; }
        [Encrypted]
        public string Name { get; set; }

        public DataModel()
        {

        }
        public DataModel(int id, string name)
        {
            ModelId = id;
            Name = name;
        }
        [ValidationAspect(typeof(DataModelValidator))]  // İçeride belirtilen valitaion kurallarını kontrol eder.
        [SecuredOperationInterceptor("admin")] // Girişi yapmış kullanıcının Roles'une göre kontrol eder.
        [CacheAspect(duration: 10)]  // Fonksiyonun döndüğü değeri kaydeder.
        [PerformanceAspect(1000)]  // 1 saniyeden yavaş olursa log düşer.
        //[CacheRemoveAspect("IDataModel.GetData")]  // Cache silmek için
        public int GetData()
        {
            return 1;
        }
    }
  
}
