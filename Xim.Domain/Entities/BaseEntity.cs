using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xim.Library.Attributes;

namespace Xim.Domain.Entities
{
    public abstract class BaseEntity<TKey> : IEntityKey<TKey>
    {
        [Key]
        public TKey id { get; set; }
    }
    public interface IEntityKey<TKey>
    {
        TKey id { get; set; }
    }
    public interface IEntityCreated
    {
        DateTime? created { get; set; }
       // bool? is_locked {  get; set; }
        //string? Ten { get; set; }
        //string? tenlink {  get; set; }
    }
    public interface IEntityModifed
    {
        DateTime? modified { get; set; }
      
    }

    /// <summary>
    /// Dùng cho các danh mục: age, weight
    /// </summary>
    public interface IDictionaryClass
    {
        int? min_value { get; set; }
        int? max_value { get; set; }
    }
}
