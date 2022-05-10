using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace BlazorServerApp1.Data
{
    //public class myClasses
    //{
    //}

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class OrderAttribute : Attribute
    {
        private readonly int order_;
        public OrderAttribute([System.Runtime.CompilerServices.CallerLineNumber]int order = 0)
        {
            order_ = order;
        }

        public int Order { get { return order_; } }
    }

    [DataContract]
    public class AGENT_Class
    {
        [DataMember(Order = 1)]
        public int AGENT { get; set; }
        [DataMember(Order = 2)]
        public string AGENTCODE { get; set; }
        [DataMember(Order = 2)]
        public string AGENTNAME { get; set; }
    }
    public class ValuesAGENT_Class
    {
        public List<AGENT_Class> value { get; set; }
    }

    [DataContract,Serializable]
    public class CUSTOMER_Class
    {
        [DataMember(Order = 1)]
        public string CUST { get; set; }
        [DataMember(Order = 2)]
        public string CUSTNAME { get; set; }
        [DataMember(Order = 3)]
        public string CUSTDES { get; set; }
        [DataMember(Order = 4)]
        public string ADDRESS { get; set; }
        [DataMember(Order = 5)]
        public string ADDRESS2 { get; set; }
        [DataMember(Order = 6)]
        public string ADDRESS3 { get; set; }
        [DataMember(Order = 7)]
        public string TRSH_LOGO { get; set; }
    }
    public class ValuesCUSTOMER_Class
    {
        public List<CUSTOMER_Class> value { get; set; }
    }

    [DataContract]
    public class FAMILY_Class
    {
        [DataMember(Order = 1)]
        public int FAMILY { get; set; }
        [DataMember(Order = 2)]
        public string FAMILYNAME { get; set; }
        [DataMember(Order = 3)]
        public string FAMILYDESC { get; set; }
    }
    public class ValuesFAMILY_Class
    {
        public List<FAMILY_Class> value { get; set; }
    }

    //PARTNAME,FAMILY,FAMILYNAME,FAMILYDES
    public class FamilyOfPart_Class
    {
        [DataMember(Order = 1)]
        public string PARTNAME { get; set; }
        // [DataMember(Order = 2)]
        // public int FAMILY { get; set; }
        [DataMember(Order = 3)]
        public string FAMILYNAME { get; set; }
        [DataMember(Order = 4)]
        public string FAMILYDES { get; set; }
    }
    public class ValuesFamilyOfPart_Class
    {
        public List<FamilyOfPart_Class> value { get; set; }
    }

    [DataContract]
    public class PART_Class
    {
        [DataMember(Order = 1)]
        public string PART { get; set; }
        [DataMember(Order = 2)]
        public string PARTNAME { get; set; }
        [DataMember(Order = 3)]
        public string PARTDES { get; set; }
        [DataMember(Order = 4)]
        public string MPARTNAME { get; set; }
        [DataMember(Order = 5)]
        public string MPARTDES { get; set; }
        [DataMember(Order = 6)]
        public int FAMILY { get; set; }
        [DataMember(Order = 7)]
        public string FAMILYNAME { get; set; }
        [DataMember(Order = 8)]
        public string FAMILYDES { get; set; }
        [DataMember(Order = 9)]
        public int TRSH_DOOR_HWCATCODE { get; set; }
    }
    public class ValuesFamilyParts
    {
        public List<PART_Class> FAMILY_LOGPART_SUBFORM { get; set; }
    }
    public class FamilyParts_Class
    {
        public List<ValuesFamilyParts> value { get; set; }
    }
    public class ValuesPART_Class
    {
        public List<PART_Class> value { get; set; }
    }


    [DataContract]
    public class TRSH_COLOR_Class
    {
        [DataMember(Order = 1)]
        public int TRSH_COLORID { get; set; }
        [DataMember(Order = 2)]
        public string TRSH_COLORNAME { get; set; }
        [DataMember(Order = 3)]
        public string TRSH_COLORDES { get; set; }
    }
    public class ValuesTRSH_COLOR_Class
    {
        public List<TRSH_COLOR_Class> value { get; set; }
    }



    [DataContract]
    //123456789 123456789
    public class TRSH_LOCKHINGE_DRILH_Class   //table 230 for field D-650 in Door title panel (גובה ניקוב \ מרכז לשונית) moved to movinwing tab
                                              // because it depends on FAMILY and DOOHEIGHT  20/11/2021
    {
        [DataMember(Order = 1)]
        public int TRSH_LOCKHINGE_DRILH { get; set; }
        [DataMember(Order = 2)]
        public int TRSH_DOOR_HWCATCODE { get; set; }
        [DataMember(Order = 3)]
        public int DOORHEIGHTMIN { get; set; }
        [DataMember(Order = 4)]
        public int DOORHEIGHTMAX { get; set; }
        [DataMember(Order = 5)]
        public string MEASURENAME { get; set; }
        [DataMember(Order = 6)]
        public int LOCKDRILHEIGHT { get; set; }
        [DataMember(Order = 7)]
        public int BACKPINHEIGHT { get; set; }
        [DataMember(Order = 8)]
        public int HINGESNUM { get; set; }
        [DataMember(Order = 9)]
        public int HINGE1HEIGHT { get; set; }
        [DataMember(Order = 10)]
        public int HINGE2HEIGHT { get; set; }
        [DataMember(Order = 11)]
        public int HINGE3HEIGHT { get; set; }
        [DataMember(Order = 12)]
        public int HINGE4HEIGHT { get; set; }
        [DataMember(Order = 13)]
        public int HINGE5HEIGHT { get; set; }
    }
    public class ValuesTRSH_LOCKHINGE_DRILH_Class
    {
        public List<TRSH_LOCKHINGE_DRILH_Class> value { get; set; }
    }

    [DataContract]
    public class TRSH_HARDWARE_Class
    {
        [DataMember(Order = 1)]
        public int TRSH_HARDWARE { get; set; }
        [DataMember(Order = 2)]
        public int TRSH_DOOR_HWCATCODE { get; set; }
        [DataMember(Order = 3)]
        public string HARDWAREDES { get; set; }
        [DataMember(Order = 4)]
        public int DRIL4HW { get; set; }
        [DataMember(Order = 4)]
        public string DRIL4HWDES { get; set; }
    }
    public class ValuesTRSH_HARDWARE_Class
    {
        public List<TRSH_HARDWARE_Class> value { get; set; }
    }

    [DataContract]
    public class DRIL4HW_Class
    {
        [DataMember(Order = 1)]
        public int DRIL4HW { get; set; }
        [DataMember(Order = 2)]
        public string DRIL4HWDES { get; set; }
    }
    public class ValuesDRIL4HW_Class
    {
        public List<DRIL4HW_Class> value { get; set; }
    }

    [DataContract]
    public class CYLINDER_Class
    {
        [DataMember(Order = 1)]
        public string CYLINDERNAME { get; set; }
        [DataMember(Order = 2)]
        public string CYLINDERDES { get; set; }
    }
    public class ValuesCYLINDER_Class
    {
        public List<CYLINDER_Class> value { get; set; }
    }

    [DataContract]
    public class TRSH_LOCK_Class
    {
        [DataMember (Order = 1)]
        public int TRSH_LOCK { get; set; }
        [DataMember(Order = 2)]
        public int TRSH_DOOR_HWCATCODE { get; set; }
        [DataMember(Order = 3)]
        public string TRSH_LOCKNAME { get; set; }
        [DataMember(Order = 4)]
        public string TRSH_LOCKDES { get; set; }
    }
    public class ValuesTRSH_LOCK_Class
    {
        public List<TRSH_LOCK_Class> value { get; set; }
    }



    [DataContract]
    public class WINDOWHEIGHT_Class  // table 140
    {
        [DataMember(Order = 1)]
        public int WINDOWHEIGHT_ID { get; set; }
        [DataMember(Order = 2)]
        public string PARTNAME { get; set; }
        [DataMember(Order = 3)]
        public int MINDOORHEIGHT { get; set; }
        [DataMember(Order = 4)]
        public int MAXDOORHEIGHT { get; set; }
        [DataMember(Order = 5)]
        public string MEASURENAME { get; set; }
        [DataMember(Order = 6)]
        public int WINDOWHEIGHT { get; set; }
        [DataMember(Order = 7)]
        public int HEIGHT4DRIL { get; set; }
    }
    public class ValuesWINDOWHEIGHT_Class
    {
        public List<WINDOWHEIGHT_Class> value { get; set; }
    }

    [DataContract]
    public class WINDOWWIDTH_Class  // table 150
    {
        [DataMember(Order = 1)]
        public int WINDOWWIDTH_ID { get; set; }
        [DataMember(Order = 2)]
        public string PARTNAME { get; set; }
        [DataMember(Order = 3)]
        public int MINDOORWIDTH { get; set; }
        [DataMember(Order = 4)]
        public int MAXDOORWIDTH { get; set; }
        [DataMember(Order = 5)]
        public int WINDOWWIDTH { get; set; }
    }
    public class ValuesWINDOWWIDTH_Class
    {
        public List<WINDOWWIDTH_Class> value { get; set; }
    }
    [DataContract]
    public class PROFILE4WINDOW_Class   //table 160
    {
        [DataMember(Order = 1)]
        //123456789 12345678
        public string PROFILE4WINDOWNAME { get; set; }
        [DataMember(Order = 2)]
        public string PROFILE4WINDOWDES { get; set; }
    }
    public class ValuesPROFILE4WINDOW_Class
    {
        public List<PROFILE4WINDOW_Class> value { get; set; }
    }


    [DataContract]
    public class TILETYPE_Class  // table 70
    {
        [DataMember(Order = 1)]
        public string TILENAME { get; set; }
        [DataMember(Order = 2)]
        public string TILEDES { get; set; }
    }
    public class ValuesTILETYPE_Class
    {
        public List<TILETYPE_Class> value { get; set; }
    }

    [DataContract]
    public class GLASS4WINDOW_Class  // table 170
    {
        [DataMember(Order = 1)]
        public int GLASS4WINDOWID { get; set; }
        [DataMember(Order = 2)]
        public string GLASS4WINDOWDES { get; set; }
        [DataMember(Order = 3)]
        public string GLASS4WINDOWKITNAME { get; set; }
    }
    public class ValuesGLASS4WINDOW_Class
    {
        public List<GLASS4WINDOW_Class> value { get; set; }
    }

    [DataContract]
    public class GRID_Class     // table 180
    {
        [DataMember(Order = 1)]
        public int GRID_ID { get; set; }
        [DataMember(Order = 2)]
        public string GRIDNAME { get; set; }
        [DataMember(Order = 2)]
        public string GRIDDES { get; set; }
    }
    public class ValuesGRID_Class
    {
        public List<GRID_Class> value { get; set; }
    }

    [DataContract]
    public class VITRAGE_Class  // table 90
    {
        [DataMember(Order = 1)]
        public string VITRAGENAME { get; set; }
        [DataMember(Order = 2)]
        public string VITRAGEDES { get; set; }
    }
    public class ValuesVITRAGE_Class
    {
        public List<VITRAGE_Class> value { get; set; }
    }

    [DataContract]
    public class RAW4CPLATES_Class  // table 110
    {
        [DataMember(Order = 1)]
        public int RAW4CPLATES { get; set; }
        [DataMember(Order = 2)]
        public string RAW4CPLATESNAME { get; set; }
        // [DataMember(Order = 3)]
        // public string RAW4CPLATEDES { get; set; }
    }
    public class ValuesRAW4CPLATES_Class
    {
        public List<RAW4CPLATES_Class> value { get; set; }
    }
    [DataContract]
    public class GRID4HT1084_Class  // table 370
    {
        [DataMember(Order = 1)]
        public string GRID4HT1084NAME { get; set; }
        [DataMember(Order = 2)]
        public string GRID4HT1084DES { get; set; }
    }
    public class ValuesGRID4HT1084_Class
    {
        public List<GRID4HT1084_Class> value { get; set; }
    }
    [DataContract]
    public class VITRAGE4DIAMOND_Class //table 250
    {
        [DataMember(Order = 1)]
        //123456789 123456789  
        public string VITRAGE4DIAMONDNAME { get; set; }
        [DataMember(Order = 2)]
        public string VITRAGE4DIAMONDDES { get; set; }
    }
    public class ValuesVITRAGE4DIAMOND_Class
    {
        public List<VITRAGE4DIAMOND_Class> value { get; set; }
    }
    [DataContract]
    public class HANDLE4DIAMOND_Class    // table 360
    {
        [DataMember(Order = 1)]
        //123456789 123456789  
        public string HANDLE4DIAMONDNAME { get; set; }
        [DataMember(Order = 2)]
        public string HANDLE4DIAMONDDES { get; set; }
        [DataMember(Order = 3)]
        public int LENGTH { get; set; }
    }
    public class ValuesHANDLE4DIAMOND_Class
    {
        public List<HANDLE4DIAMOND_Class> value { get; set; }
    }

    [DataContract]
    public class HANDLE_Class    // table 360
    {
        [DataMember(Order = 1)]
        //123456789 123456789  
        public string HANDLENAME { get; set; }
        [DataMember(Order = 2)]
        public string HANDLEDES { get; set; }
        [DataMember(Order = 3)]
        public int LENGTH { get; set; }
    }
    public class ValuesHANDLE_Class
    {
        public List<HANDLE_Class> value { get; set; }
    }

    [DataContract]
    public class ConfField_Class
    {
        [DataMember(Order = 1)]
        public int FIELDID { get; set; }
        [DataMember(Order = 2)]
        public string CONFIG_FIELDNAME { get; set; }
        [DataMember(Order = 3)]
        public string CONFIG_THNAME { get; set; }
        [DataMember(Order = 4)]
        public string CONFIG_TDNAME { get; set; }
        [DataMember(Order = 5)]
        public string FIELDDATATYPE { get; set; }
        [DataMember(Order = 6)]
        public string FIELDNAME { get; set; }
        [DataMember(Order = 7)]
        public string FIELDCODE { get; set; }
        [DataMember(Order = 8)]
        public string FIELDDES { get; set; }
        [DataMember(Order = 9)]
        public string CONFIG_SUBFORM { get; set; }
    }

    public class ValuesConfField_Class
    {
        public List<ConfField_Class> value { get; set; }
    }

    [DataContract]
    public class MeagedFields_Class
    {
        [DataMember(Order = 1)]
        public string MEAGEDNAME { get; set; }  //PARTNAME of פריט מרכז
        [DataMember(Order = 2)]
        public string CONFIG_THNAME { get; set; }
        [DataMember(Order = 3)]
        public string CONFIG_TDNAME { get; set; }
        [DataMember(Order = 4)]
        public string FIELDCODE { get; set; }
        [DataMember(Order = 5)]
        public string CONFIG_FIELDNAME { get; set; }
        [DataMember(Order = 6)]
        public bool Visible { get; set; }
    }
    public class ValuesMeagedFields_Class
    {
        public List<MeagedFields_Class> value { get; set; }
    }

    [DataContract]
    public class DecorSideFlds_Class
    {
        [DataMember(Order = 1)]
        public string DECORSIDECODE { get; set; }
        [DataMember(Order = 2)]
        public string DECORSIDE { get; set; }
        [DataMember(Order = 3)]
        public string CONFIG_THNAME { get; set; }
        [DataMember(Order = 4)]
        public string CONFIG_TDNAME { get; set; }
        [DataMember(Order = 5)]
        public string FIELDCODE { get; set; }
        [DataMember(Order = 6)]
        public string CONFIG_FIELDNAME { get; set; }
        [DataMember(Order = 7)]
        public string SHOW { get; set; }
    }
    public class ValuesDecorSideFlds_Class
    {
        public List<DecorSideFlds_Class> value { get; set; }
    }

    [DataContract]
    public class Defaults_Class
    {
        [DataMember(Order = 1)]
        public int LINENUM { get; set; }
        [DataMember(Order = 2)]
        public int TRSH_NUM { get; set; }
        [DataMember(Order = 3)]
        public string PARTNAME { get; set; }
        [DataMember(Order = 4)]
        public string FIELDCODE { get; set; }
        [DataMember(Order = 5)]
        public string CONFIG_FIELDNAME { get; set; }  //e.g. dlstPARTNAME
        [DataMember(Order = 6)]
        public string FIELDNAME { get; set; }  //e.g. PARTNAME
        [DataMember(Order = 7)]
        public string CONFIG_TDNAME { get; set; }
        [DataMember(Order = 8)]
        public string FIELDDATATYPE { get; set; }
        [DataMember(Order = 9)]
        public string DEFVAL { get; set; }
        [DataMember(Order = 10)]
        public string VAL_LOCKED { get; set; }
        [DataMember(Order = 11)]
        public string WRONGVAL { get; set; }
    }
    public class ValuesDefaults_Class
    {
        public List<Defaults_Class> value { get; set; }
    }

    [DataContract, Serializable]
    public class DoorConfig
    {
        #region form header
        [DataMember(Order = 1), Order]
        public string REFERENCE { get; set; }
        //[Required]
        [DataMember(Order = 2), Order]
        public string FORMDATE { get; set; }
        [DataMember(Order = 3), Order]
        public string FORMFILLER { get; set; }
        [DataMember(Order = 4), Order]
        public int AGENT { get; set; }
        //[DataMember(Order = 5), Order]
        //public string AGENTCODE { get; set; }
        //[DataMember(Order = 5), Order]
        //public string AGENTNAME { get; set; }

        //[Required]      
        [DataMember(Order = 6), Order]
        public int CUST { get; set; }
        //[DataMember(Order = 6), Order]
        //public string CUSTNAME { get; set; }
        //[DataMember(Order = 7), Order]
        //public string CUSTDES { get; set; }
        [DataMember(Order = 8), Order]
        public string ADDRESS { get; set; }
        [DataMember(Order = 9), Order]
        public string SHIPADDRESS { get; set; }
        //[DataMember(Order = 10), Order]
        //public string ADDRESS3 { get; set; }
        #endregion form header

        #region door header
        
        [DataMember(Order = 12), Order]
        public string WINGSNUM { get; set; }  // one, one and half, double 
        [DataMember(Order = 13), Order]
        public string OPENMODE { get; set; }  // in,out  D-40
        [DataMember(Order = 14), Order]
        public string COMPLIENTDOOR { get; set; }  // Y | N | '' D-920
        //[DataMember(Order = 23), Order]
        //public int PART { get; set; }  //D-20
        //[Required]
        [DataMember(Order = 15), Order]
        public string PARTNAME { get; set; }  //D-20
        //[DataMember(Order = 24), Order]
        //public string PARTDES { get; set; }
        [DataMember(Order = 16), Order]
        public int FAMILY { get; set; }  // D-10
        //[DataMember(Order = 22), Order]
        //public string FAMILYDES { get; set; }
        [NonSerialized]
        public string FAMILYNAME = string.Empty;
        [DataMember(Order = 17), Order]
        public int TRSH_DOOR_HWCATCODE { get; set; }
        #endregion door header

        #region moving wing
        [DataMember(Order = 27), Order]
        public int LOCKDRILHEIGHT { get; set; }        //D-650
       
        [DataMember(Order = 29), Order]
        public string OPENSIDE { get; set; }  //R, L    D-50
        [DataMember(Order = 40), Order]
        public string DECORFORMAT { get; set; }      // D-60
        [DataMember(Order = 41), Order]
        public string TWOCOLORS { get; set; }  // Y | N | '' D-70
        [DataMember(Order = 42), Order]
        public int DOORWIDTH { get; set; }           // D-80
        [DataMember(Order = 43), Order]
        public int DOORCOLORID { get; set; }      // D-110
        //[DataMember(Order = 44), Order]
        //public string DOORCOLORDES { get; set; }
        [DataMember(Order = 45), Order]
        public int DOORHEIGHT { get; set; }         // D-570
        [DataMember(Order = 46), Order]
        public string TURBOAPPARATUS { get; set; } //Y, N, ''  //D-660
        [DataMember(Order = 47), Order]
        public string LOGO { get; set; }              // D-680
        [DataMember(Order = 48), Order]
        public int TRSH_HARDWARE { get; set; }  // Pirzul  D-870
        //[DataMember(Order = 49), Order]
        //public string HARDWAREDES { get; set; }
        [DataMember(Order = 50), Order]
        public int HWCOLORID { get; set; }      // D-880
        //[DataMember(Order = 60), Order]
        //public string HWCOLORDES { get; set; }
        [DataMember(Order = 61), Order]
        public int DRIL4HW { get; set; }  //D-890 table 300 DRIL4HW (DRIL4HW, DRIL4HWDES)
        //[DataMember(Order = 62), Order]
        //public string DRIL4HWDES { get; set; }
        [DataMember(Order = 63), Order]
        public string CYLINDERNAME { get; set; }  //D-900  , table 310
        //[DataMember(Order = 64), Order]
        //public string CYLINDERDES { get; set; }
        [DataMember(Order = 65), Order]
        public string ELECTRICAPPARATUS { get; set; } // Y, N, ''  D-910
        [DataMember(Order = 66), Order]
        public string LOCKNAME { get; set; }   // D-930
        //[DataMember(Order = 67), Order]
        //public string LOCKDES { get; set; }
        [DataMember(Order = 68), Order]
        public string DRIL4TWOSIDESIDS { get; set; }   //D-980  Y, N ,'' - // previous טבלת ניקוב לפירזול  300  Dril 4 two sides IDS      ניקוב שני צדדים IDS
        [DataMember(Order = 69), Order]
        public string RAFAFAONMOVINGWING { get; set; }  // D-1010
        [DataMember(Order = 70), Order]
        public string LCDEYE { get; set; }  // Y, N, ''  //D-1030
        #endregion moving wing

        #region Ext Decoration (extdecor )
        #region extDecorL1
        [DataMember(Order = 80), Order]
        public string LINESFORMAT { get; set; }  //D-90  Shkuim Boltin שקועים \ בולטים 
        [DataMember(Order = 81), Order]
        public string TILENAME { get; set; }      // D-100
        //[DataMember(Order = 82), Order]
        //public string TILEDES { get; set; }
        [DataMember(Order = 83), Order]
        public int EXTCOLORID { get; set; }   //D-120
        //[DataMember(Order = 84), Order]
        //public string EXTCOLORDES { get; set; }
        [DataMember(Order = 85), Order]
        public int GRIDCOLORID { get; set; }   // D-140
        //[DataMember(Order = 86), Order]
        //public string GRIDCOLORDES { get; set; }
        [DataMember(Order = 87), Order]
        public string VITRAGECLRBYCTLG { get; set; }  // Y, N , ''  D-150
        [DataMember(Order = 88), Order]
        public int EXTSEPLINESCLRID { get; set; }  //D-160
        //[DataMember(Order = 89), Order]
        //public string EXTSEPLINESCLRDES { get; set; }  
        [DataMember(Order = 90), Order]
        public int EXTCPLATEHTDMNDCLRID { get; set; }   //D-170
        //[DataMember(Order = 100), Order]
        //public string EXTCPLATEHTDMNDCLDES { get; set; }
        [DataMember(Order = 101), Order]
        public int EXTPERIFPROFILECLRID { get; set; }  // D-180
        //[DataMember(Order = 102), Order]
        //public string EXTPERIFPROFILECLDES { get; set; }
        [DataMember(Order = 103), Order]
        public int EXTMODERNCPLATECLRID { get; set; }  //D-250  
        //[DataMember(Order = 104), Order]
        //public string EXTMODERNCPLATECLDES { get; set; }
        [DataMember(Order = 105), Order]
        public int EXTCGTIDCLRID { get; set; }    // D-260
                                                  //[DataMember(Order = 106), Order]
                                                  //public string EXTCGTIDCLRDES { get; set; }
        #endregion extDecorL1
        #region extDecorL2
        [DataMember(Order = 107), Order]
        public int EXTHTPLATESCLRID { get; set; }  // D-270
        //[DataMember(Order = 108), Order]
        //public string EXTHTPLATESCLRDES { get; set; }
        [DataMember(Order = 109), Order]
        public int ATTACHEDPLATECLRID { get; set; }  // D-280
        [DataMember(Order = 110), Order]
        public string ATTACHEDPLATECLRDES { get; set; }
        [DataMember(Order = 120), Order]
        public int EXTGLASSPLATECLRID { get; set; }  // D-310
        [DataMember(Order = 121), Order]
        //public string EXTGLASSPLATECLRDES { get; set; }
        //[DataMember(Order = 122), Order]
        public int EXTMODERNPLATECLRID { get; set; }  // D-330
        [DataMember(Order = 123), Order]
        public string EXTMODERNPLATECLRDES { get; set; }
        [DataMember(Order = 124), Order]
        public int DECORPLATECLRID { get; set; }  //D-350
        [DataMember(Order = 125), Order]
        public string DECORPLATECLRDES { get; set; }
        [DataMember(Order = 126), Order]
        public int NIROSTALINESCLRID { get; set; }  //D-360
        [DataMember(Order = 127), Order]
        public string NIROSTALINESCLRDES { get; set; }
        [DataMember(Order = 128), Order]
        public int EXTNIROSTALINESCLRID { get; set; }  //D-380
        [DataMember(Order = 129), Order]
        public string EXTNIROSTALINESCDES { get; set; }
        [DataMember(Order = 130), Order]
        public int EXTINSERTCLRID { get; set; }   // D-410
        [DataMember(Order = 140), Order]
        public string EXTINSERTCLRDES { get; set; }
        [DataMember(Order = 141), Order]
        public string VITRAGEPATTERNONDOOR { get; set; } // Y, N, ''  D-430
        [DataMember(Order = 142), Order]
        public int WINDOWWIDTH { get; set; }  //D-440
        [DataMember(Order = 143), Order]
        public int WINDOWHEIGHT { get; set; }       // D-450
        #endregion extDecorL2
        #region extDecorL3
        [DataMember(Order = 144), Order]
        public string PROFILE4WINDOWNAME { get; set; }  //D-460
        //[DataMember(Order = 145), Order]
        //public string PROFILE4WINDOWDES { get; set; }
        [DataMember(Order = 146), Order]
        public string DECORFRAME { get; set; }  //D-470  Y, N, ''
        [DataMember(Order = 147), Order]
        public int GLASS4WINDOWID { get; set; }  // D-480
        //[DataMember(Order = 148), Order]
        //public string GLASS4WINDOWDES { get; set; }
        [DataMember(Order = 149), Order]
        public int GRID_ID { get; set; }  //D-490
        //[DataMember(Order = 150), Order]
        //public string GRIDDES { get; set; }
        [DataMember(Order = 160), Order]
        public string VITRAGENAME { get; set; } //D-500
        //[DataMember(Order = 161), Order]
        //public string VITRAGEDES { get; set; }
        [DataMember(Order = 162), Order]
        public string JAPANESEWINDOW { get; set; }  // Y, N, ''  //D-550
        [DataMember(Order = 163), Order]
        public string PLATESFOR7504 { get; set; }  //Y, N, ''  //D-690
        [DataMember(Order = 164), Order]
        public string EXTCPLATE4HTDMNDNAME { get; set; } //D-700
        [DataMember(Order = 165), Order]
        public string EXTSIDE_C_PLATENAME { get; set; }   //D-720
        [DataMember(Order = 166), Order]
        public string DECORGRIDPLATEDES { get; set; }  //D-740
        #endregion extDecorL3
        #region extDecorL4
        [DataMember(Order = 167), Order]
        public string GRID4HT1084NAME { get; set; } // hidden D-750  ?? - this is the U-key of the table GRID4HT1084 to be created (14/11/2021 )
                                                    // on the form the name of the field id is dlstDecorPlate  and it's th is 
                                                    // דוגמא לוח דקורטיבי  , i.e. 1084 is not present in the field name !
                                                    //   maybe the Hebrew header/title of the field is wrong.
                                                    //  1084 is mentioned in the data source for this dropDownList control.
                                                    //  but, since this is the only control that uses this table - the field name in the DoorConfig record
                                                    //   will be GRID4HT1084NAME .
                                                    //[DataMember(Order = 168), Order]
                                                    //public string GRID4HT1084DES { get; set; } // D-750 ?? displayed
        [DataMember(Order = 169), Order]
        public string EXTGRIDCPLATEDES { get; set; }  // D-760
        [DataMember(Order = 170), Order]
        public string EXTHTPLATENAME { get; set; }  // D-770
        [DataMember(Order = 180), Order]
        public string VITRAGE4DIAMONDNAME { get; set; }  // D-800
        //[DataMember(Order = 181), Order]
        //public string VITRAGE4DIAMONDDES { get; set; }
        [DataMember(Order = 182), Order]
        public int EXTVITRAGE4DIAMONDQ { get; set; }  // D-810
        [DataMember(Order = 183), Order]
        public int INTERPLATESSPACE { get; set; } // D-830
        [DataMember(Order = 184), Order]
        public string EXTFINMODERNCPLATE { get; set; } //D-850  חלק , מחורץ
        [DataMember(Order = 185), Order]
        public string EXTFINMODERNSEPLINE { get; set; }  // D-970
        [DataMember(Order = 186), Order]
        public string EXTFINMODERNPLATE { get; set; }  // D-1000
        [DataMember(Order = 187), Order]
        public string HANDLE4DIAMONDNAME { get; set; } // D-1160
                                                       //[DataMember(Order = 188), Order]
                                                       //public string HANDLE4DIAMONDDES { get; set; }
        #endregion extDecorL4
        #endregion Ext Decoration (extdecor )

        #region Internal decoration (intdecor)
        #region Internal decoration L1
        [DataMember(Order = 189), Order]
        public int INTCOLORID { get; set; }   //D-130
        [DataMember(Order = 190), Order]
        public string INTCOLORDES { get; set; }
        [DataMember(Order = 200), Order]
        public int INTCPLATEHTDMNDCLRID { get; set; }  //D-190
        [DataMember(Order = 201), Order]
        public string INTCPLATEHTDMNDCLDES { get; set; }
        [DataMember(Order = 202), Order]
        public int INTPERIFPROFILECLRID { get; set; }  //D-200
        //[DataMember(Order = 203), Order]
        //public string INTPERIFPROFILECLDES { get; set; }
        [DataMember(Order = 204), Order]
        public int INTSIDECNTRPLATECLID { get; set; }                  //D-220
        [DataMember(Order = 205), Order]
        public string INTSIDECNTRPLATECDES { get; set; }
        [DataMember(Order = 206), Order]
        public int INTSEPLINESCLRID { get; set; }           //D-230
        [DataMember(Order = 207), Order]
        public string INTSEPLINESCLRDES { get; set; }
        [DataMember(Order = 208), Order]
        public int INTMODERNCPLATECLRID { get; set; }      //D-240
        [DataMember(Order = 209), Order]
        public string INTMODERNCPLATECLDES { get; set; }
        [DataMember(Order = 210), Order]
        public int INTCGRIDCLRID { get; set; }  //D-290
        [DataMember(Order = 220), Order]
        public string INTCGRIDCLRDES { get; set; }
        #endregion Internal decoration L1

        #region Internal decoration L2
        [DataMember(Order = 221), Order]
        public int INTHTPLATESCLRID { get; set; }  //D-300
        [DataMember(Order = 222), Order]
        public string INTHTPLATESCLRDES { get; set; }
        [DataMember(Order = 223), Order]
        public int INTGLASSPLATECLRID { get; set; }  //D-320
        //[DataMember(Order = 224), Order]
        //public string INTGLASSPLATECLRDES { get; set; }
        [DataMember(Order = 225), Order]
        public int INTMODERNPLATECLRID { get; set; }  //D-340
        [DataMember(Order = 226), Order]
        public string INTMODERNPLATECLRDES { get; set; }
        [DataMember(Order = 227), Order]
        public int INTNIROSTALINESCLRID { get; set; } //D-370
        [DataMember(Order = 228), Order]
        public string INTNIROSTALINESCLDES { get; set; }
        [DataMember(Order = 229), Order]
        public int INTINSERTCLRID { get; set; }   //D-420
        [DataMember(Order = 230), Order]
        public string INTINSERTCLRDES { get; set; }
        [DataMember(Order = 240), Order]
        public string INTCPLATE4HTDMNDNAME { get; set; }  //D-710
        [DataMember(Order = 241), Order]
        public string INTSIDE_C_PLATE { get; set; }  //D-730
        #endregion Internal decoration L2

        #region Internal decoration L3
        [DataMember(Order = 242), Order]
        public string INTGRIDCPLATE { get; set; } //D-780
        [DataMember(Order = 243), Order]
        public string INTHTPLATE { get; set; } //D-790
        [DataMember(Order = 244), Order]
        public int INTVITRAGE4DIAMONDQ { get; set; } //D-820
        [DataMember(Order = 245), Order]
        public string INTFINMODERNCPLATE { get; set; } //D-840
        [DataMember(Order = 246), Order]
        public string INTFINMODERNSEPLINE { get; set; } //D-960
        [DataMember(Order = 247), Order]
        public string INTFINMODERNPLATE { get; set; } //D-990

        #endregion Internal decoration L3
        #endregion Internal decoration (intdecor)

        #region staticwing
        [DataMember(Order = 260), Order]
        public int CENTRALCOLCLRID { get; set; }         //D-390
        [DataMember(Order = 261), Order]
        public string CENTRALCOLCLRDES { get; set; }
        [DataMember(Order = 262), Order]
        public int SHALVANIACLRID { get; set; }          //D-400
        [DataMember(Order = 263), Order]
        public string SHALVANIACLRDES { get; set; }
        [DataMember(Order = 264), Order]
        public string CYLINDER4HALFWING { get; set; }    //D-510
        [DataMember(Order = 265), Order]
        public string HW4EXTRAWING { get; set; }         //D-520  Y, N , ''
        [DataMember(Order = 266), Order]
        public string DESIGNEDEXTRAWING { get; set; }    //D-530  Y, N , ''
        [DataMember(Order = 267), Order]
        public string DESIGNEDWINDOWEDWING { get; set; } //D-540   Y, N , ''
        [DataMember(Order = 268), Order]
        public int EXTRAWINGWIDTH { get; set; }          //D-560
        [DataMember(Order = 269), Order]
        public int CENTRALCOLWIDTH { get; set; }         //D-860
        [DataMember(Order = 270), Order]
        public string RAFAFAONSTATICWING { get; set; }   //D-1150  Y, N , ''
        #endregion staticwing { get; set; }
        #region hinges
        [DataMember(Order = 280), Order]
        public int HINGESNUM { get; set; }      //D-580
        [DataMember(Order = 281), Order]
        public int BACKPINHEIGHT { get; set; }  //D-590
        [DataMember(Order = 282), Order]
        public int HINGE1HEIGHT { get; set; }   //D-600
        [DataMember(Order = 283), Order]
        public int HINGE2HEIGHT { get; set; }   //D-610
        [DataMember(Order = 284), Order]
        public int HINGE3HEIGHT { get; set; }   //D-620
        [DataMember(Order = 285), Order]
        public int HINGE4HEIGHT { get; set; }   //D-630
        [DataMember(Order = 286), Order]
        public int HINGE5HEIGHT { get; set; }   //D-640
        #endregion hinges
        #region handle
        [DataMember(Order = 290), Order]
        public string HANDLENAME { get; set; }   //D-940
        [DataMember(Order = 291), Order]
        public int HANDLECOLORID { get; set; }   //D-950
        [DataMember(Order = 292), Order]
        public string HANDLECOLORDES { get; set; }
        #endregion handle

        #region accessories
        [DataMember(Order = 300), Order]
        public string RAFAFALOC { get; set; }
        [DataMember(Order = 301), Order]
        public string SAFELOCK { get; set; }   //D-1040
        [DataMember(Order = 302), Order]
        public string WETCOLORBOX { get; set; }  //D-1050
        [DataMember(Order = 303), Order]
        public string COLORSPRAY { get; set; }   //D-1060
        [DataMember(Order = 304), Order]
        public string CLEANINGSPRAY { get; set; }  //D-1070
        [DataMember(Order = 305), Order]
        public string CORE { get; set; }           //D-1080
        [DataMember(Order = 306), Order]
        public string MEZUZA { get; set; }         //D-1090
        #endregion accessories
        [DataMember(Order = 350), Order]
        public string COMMENTS { get; set; } = string.Empty;
        [NonSerialized]
        public string meaged = string.Empty;
        //[NonSerialized]
        public Dictionary<string, string> borderColors { get; set; } 

        public void initBorderColors()
        {
            Type objType = this.GetType();
            PropertyInfo[] props = objType.GetProperties();
            borderColors = new Dictionary<string, string>();
            foreach (PropertyInfo pinfo in props)
            {
                borderColors.Add(pinfo.Name, string.Empty);
            }
        }

    }
    public class ValuesDoorConfig
    {
        public List<DoorConfig> value { get; set; }
    }

    

    //public class ReadOnlyDictionary<TKey, TValue>
    //{
    //    private Dictionary<TKey, TValue> _dict;
    //    public ReadOnlyDictionary(Dictionary<TKey, TValue> dict)
    //    {
    //        _dict = dict;
    //    }

    //    public TValue this[TKey key] { get { return _dict[key]; } }
    //}
    public class UserData
    {
        public string TabId { get; set; }
    }
    public class SessionInfo
    {
        public string SessionId { get; set; }
        public string compName { get; set; }
    }
   
}
