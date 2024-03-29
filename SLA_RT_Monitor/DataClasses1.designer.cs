﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LinqExample
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Database1")]
	public partial class DataClasses1DataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertUser(User instance);
    partial void UpdateUser(User instance);
    partial void DeleteUser(User instance);
    partial void InsertUserType(UserType instance);
    partial void UpdateUserType(UserType instance);
    partial void DeleteUserType(UserType instance);
    #endregion
		
		public DataClasses1DataContext() : 
				base(global::LinqExample.Properties.Settings.Default.Database1ConnectionString1, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<User> Users
		{
			get
			{
				return this.GetTable<User>();
			}
		}
		
		public System.Data.Linq.Table<UserType> UserTypes
		{
			get
			{
				return this.GetTable<UserType>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Users")]
	public partial class User : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Users_ID;
		
		private string _Users_Username;
		
		private System.Nullable<int> _Users_UserType_FK;
		
		private string _Users_Pass;
		
		private EntityRef<UserType> _UserType;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnUsers_IDChanging(int value);
    partial void OnUsers_IDChanged();
    partial void OnUsers_UsernameChanging(string value);
    partial void OnUsers_UsernameChanged();
    partial void OnUsers_UserType_FKChanging(System.Nullable<int> value);
    partial void OnUsers_UserType_FKChanged();
    partial void OnUsers_PassChanging(string value);
    partial void OnUsers_PassChanged();
    #endregion
		
		public User()
		{
			this._UserType = default(EntityRef<UserType>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Users_ID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int Users_ID
		{
			get
			{
				return this._Users_ID;
			}
			set
			{
				if ((this._Users_ID != value))
				{
					this.OnUsers_IDChanging(value);
					this.SendPropertyChanging();
					this._Users_ID = value;
					this.SendPropertyChanged("Users_ID");
					this.OnUsers_IDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Users_Username", DbType="NChar(10)")]
		public string Users_Username
		{
			get
			{
				return this._Users_Username;
			}
			set
			{
				if ((this._Users_Username != value))
				{
					this.OnUsers_UsernameChanging(value);
					this.SendPropertyChanging();
					this._Users_Username = value;
					this.SendPropertyChanged("Users_Username");
					this.OnUsers_UsernameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Users_UserType_FK", DbType="Int")]
		public System.Nullable<int> Users_UserType_FK
		{
			get
			{
				return this._Users_UserType_FK;
			}
			set
			{
				if ((this._Users_UserType_FK != value))
				{
					this.OnUsers_UserType_FKChanging(value);
					this.SendPropertyChanging();
					this._Users_UserType_FK = value;
					this.SendPropertyChanged("Users_UserType_FK");
					this.OnUsers_UserType_FKChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Users_Pass", DbType="NChar(10)")]
		public string Users_Pass
		{
			get
			{
				return this._Users_Pass;
			}
			set
			{
				if ((this._Users_Pass != value))
				{
					this.OnUsers_PassChanging(value);
					this.SendPropertyChanging();
					this._Users_Pass = value;
					this.SendPropertyChanged("Users_Pass");
					this.OnUsers_PassChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="User_UserType", Storage="_UserType", ThisKey="Users_ID", OtherKey="UserTypes_ID", IsUnique=true, IsForeignKey=false)]
		public UserType UserType
		{
			get
			{
				return this._UserType.Entity;
			}
			set
			{
				UserType previousValue = this._UserType.Entity;
				if (((previousValue != value) 
							|| (this._UserType.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._UserType.Entity = null;
						previousValue.User = null;
					}
					this._UserType.Entity = value;
					if ((value != null))
					{
						value.User = this;
					}
					this.SendPropertyChanged("UserType");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.UserTypes")]
	public partial class UserType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _UserTypes_ID;
		
		private string _UserTypes_Type;
		
		private EntityRef<User> _User;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnUserTypes_IDChanging(int value);
    partial void OnUserTypes_IDChanged();
    partial void OnUserTypes_TypeChanging(string value);
    partial void OnUserTypes_TypeChanged();
    #endregion
		
		public UserType()
		{
			this._User = default(EntityRef<User>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserTypes_ID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int UserTypes_ID
		{
			get
			{
				return this._UserTypes_ID;
			}
			set
			{
				if ((this._UserTypes_ID != value))
				{
					if (this._User.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnUserTypes_IDChanging(value);
					this.SendPropertyChanging();
					this._UserTypes_ID = value;
					this.SendPropertyChanged("UserTypes_ID");
					this.OnUserTypes_IDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserTypes_Type", DbType="NChar(10)")]
		public string UserTypes_Type
		{
			get
			{
				return this._UserTypes_Type;
			}
			set
			{
				if ((this._UserTypes_Type != value))
				{
					this.OnUserTypes_TypeChanging(value);
					this.SendPropertyChanging();
					this._UserTypes_Type = value;
					this.SendPropertyChanged("UserTypes_Type");
					this.OnUserTypes_TypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="User_UserType", Storage="_User", ThisKey="UserTypes_ID", OtherKey="Users_ID", IsForeignKey=true)]
		public User User
		{
			get
			{
				return this._User.Entity;
			}
			set
			{
				User previousValue = this._User.Entity;
				if (((previousValue != value) 
							|| (this._User.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._User.Entity = null;
						previousValue.UserType = null;
					}
					this._User.Entity = value;
					if ((value != null))
					{
						value.UserType = this;
						this._UserTypes_ID = value.Users_ID;
					}
					else
					{
						this._UserTypes_ID = default(int);
					}
					this.SendPropertyChanged("User");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
