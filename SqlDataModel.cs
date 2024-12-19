using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace YourNameSpace
{
  public abstract class BaseEntity : INotifyPropertyChanged
  {
      public event PropertyChangedEventHandler PropertyChanged;
  
      protected virtual void OnPropertyChanged(string propertyName)
      {
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
      //  屬性變更通知
      protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
      {
          if (EqualityComparer<T>.Default.Equals(field, value))
              return false;
  
          field = value;
          OnPropertyChanged(propertyName);
          return true;
      }
  }
  [Table("XXX")]
  public class XXX : BaseEntity
  {
      private string _no;
      [Key]
      [StringLength(10)]
      public string no
      {
          get => _no;
          set => SetProperty(ref _no, value);
      }
  
      private string _address;
      [Required]
      [StringLength(70)]
      public string address
      {
          get => _address;
          set => SetProperty(ref _address, value);
      }
  }
}
