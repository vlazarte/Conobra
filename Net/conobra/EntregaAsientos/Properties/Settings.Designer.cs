﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartQuickbook.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("b4f16ca3cd7d_")]
        public string qbook_production {
            get {
                return ((string)(this["qbook_production"]));
            }
            set {
                this["qbook_production"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("CONOBRA LTDA.")]
        public string qbook_CompaniaBD {
            get {
                return ((string)(this["qbook_CompaniaBD"]));
            }
            set {
                this["qbook_CompaniaBD"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("CONOBRA LTDA. - Quickbooks Factura Electrónica Costa Rica.")]
        public string qbook_Compania {
            get {
                return ((string)(this["qbook_Compania"]));
            }
            set {
                this["qbook_Compania"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("CONOBRA LTDA SmartStrategy - Quickbook Connector v2.4")]
        public string qbook_app_name {
            get {
                return ((string)(this["qbook_app_name"]));
            }
            set {
                this["qbook_app_name"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Quickbooks db\\Conobra\\Conobra Limitada.qbw")]
        public string qbook_file {
            get {
                return ((string)(this["qbook_file"]));
            }
            set {
                this["qbook_file"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://smartstrategyonline.com/Cloud/WebServices/IntegracionesQuickbook/WService." +
            "asmx")]
        public string SmartQuickbook_wClient_WService {
            get {
                return ((string)(this["SmartQuickbook_wClient_WService"]));
            }
        }
    }
}
