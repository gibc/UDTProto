using ExpoTest.Properties;
using Prism.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ExpoTest
{

    public class RequiredValidate : ValidationRule
    {
        //public RequiredValidate()
        //{
        //    ValidatesOnTargetUpdated = true;
        //}
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strVal = value as string;
            return strVal != null && strVal.Length > 0 ? ValidationResult.ValidResult : new ValidationResult(false, "Value required");
        }
    }

    public class Expo //: INotifyPropertyChanged
    {

        public DelegateCommand MyICommandThatShouldHandleLoaded { get; set; }
        private void winLoaded()
        {
            TextBox txtValue = new TextBox();

            txtValue.Height = 30;
            txtValue.Width = 150;

            Binding binding = new Binding();
            binding.Path = new PropertyPath("exObj.NewProp");
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            binding.ValidatesOnDataErrors = true;
            binding.NotifyOnValidationError = true;
            binding.Mode = BindingMode.TwoWay;
            RequiredValidate role = new RequiredValidate();
            binding.ValidationRules.Add(role);
            txtValue.SetBinding(TextBox.TextProperty, binding);

            View.AddTextBoxToGrid(txtValue);
        }

        public static DependencyProperty GetDependencyPropertyByName(DependencyObject dependencyObject, string dpName)
        {
            return GetDependencyPropertyByName(dependencyObject.GetType(), dpName);
        }

        public static DependencyProperty GetDependencyPropertyByName(Type dependencyObjectType, string dpName)
        {
            DependencyProperty dp = null;

            var fieldInfo = dependencyObjectType.GetField(dpName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (fieldInfo != null)
            {
                dp = fieldInfo.GetValue(null) as DependencyProperty;
            }

            return dp;
        }

        public dynamic exObj { get; set; }
        public DataView dataView
        {
            get 
            {
                return getTbl();
            }
            set { dataView = value; } 
        }

        public IView View { get; set; }

        DataView getTbl()
        {
            DataTable tbl = new DataTable();

            var rowOne = exObj.Col[0];

            IDictionary<string, object> propertyValues = (IDictionary<string, object>)rowOne;
            foreach (var prop in propertyValues)
            {
                //tbl.Columns.Add(prop.Key, typeof(string));
                tbl.Columns.Add(prop.Key, prop.Value.GetType());
            }

            foreach (var obj in exObj.Col)
            {
                DataRow row = tbl.NewRow();

                int index = 0;
                IDictionary<string, object> propValues = (IDictionary<string, object>)obj;
                foreach (var prop in propValues)
                {
                    row.SetField(index++, prop.Value.ToString());
                }

                tbl.Rows.Add(row);
            }

            return tbl.DefaultView;


        }
        public Expo()
        {
            MyICommandThatShouldHandleLoaded = new DelegateCommand(winLoaded);
            exObj = new ExpandoObject();

            //exObj.Add("NewProp", string.Empty);
            //exObj.NewProp = "some more new text";
            ((IDictionary<string, object>)exObj).Add("NewProp", "some old text");

            exObj.Col = new List<ExpandoObject>();
            dynamic item = new ExpandoObject();
            item.Name = "One";
            item.Number = 100.44;
            item.Part = "Wheel";
            item.Count = 10;
            exObj.Col.Add(item);

            var iobj = exObj as IDictionary<string, object>;
            iobj.Add("strinProp", 22);

            item = new ExpandoObject();
            item.Name = "Two";
            item.Number = 200;
            item.Part = "Tire";
            item.Count = 17;            
            exObj.Col.Add(item);

            item = new ExpandoObject();
            item.Name = "Three";
            item.Number = 300;
            item.Part = "Hub Cap";
            item.Count = 1000;           
            exObj.Col.Add(item);

            item = new ExpandoObject();
            item.Name = "Four";
            item.Number = 400;
            item.Part = "Spark Plug";
            item.Count = 8;
            exObj.Col.Add(item);

            item = new ExpandoObject();
            item.Name = "Five";
            item.Number = 500;
            item.Part = "Radiator";
            item.Count = 3976;
            exObj.Col.Add(item);

        }

        public void SetText(string val)
        {
            exObj.NewProp = val;
        }

        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            //Take use of the IDictionary implementation
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
