namespace YAPI.suburban
{
    using System.Xml.Serialization;
    using System.Text;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class startup
    {

        private string uuidField;

        private startupApp[] appField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string uuid
        {
            get
            {
                return this.uuidField;
            }
            set
            {
                this.uuidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("app", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public startupApp[] app
        {
            get
            {
                return this.appField;
            }
            set
            {
                this.appField = value;
            }
        }
        public static startup FromXml(htmlRetrieval.WebPage page)
        {
            string html = page.Html;
            if (page.ErrorsInRequest)
                return null;
            byte[] xmldata = Encoding.UTF8.GetBytes(html);
            return xml.FromXML<startup>(xmldata);
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class startupApp
    {

        private string cur_app_versionField;

        private string min_app_versionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cur_app_version
        {
            get
            {
                return this.cur_app_versionField;
            }
            set
            {
                this.cur_app_versionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string min_app_version
        {
            get
            {
                return this.min_app_versionField;
            }
            set
            {
                this.min_app_versionField = value;
            }
        }
    }
}