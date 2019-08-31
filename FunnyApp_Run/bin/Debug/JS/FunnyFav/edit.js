
var userName="none";
var log_msg="";

var msg_id=0;
var myMap=[];

String.prototype.replaceAll = function (FindText, RepText) {
    regExp = new RegExp(FindText, "g");
    return this.replace(regExp, RepText);
}

//消息和发送计数器
function C_Msg(ID,Msg){
    this.ID=ID;
    this.Msg=Msg;
    this.Count=0;
}

function C_Link(name,url){
    this.name=name;
    this.url=url;
}


function text_keydown(data){
    if (data==13){
        send_msg_click();
    }
}


function read_ini(){
    //sys.Combox_Clear("cb_friend");
    var path=sys.AppPath();
    var strCount=sys.Ini_Read(path+"\\config\\friend.ini","items","count");
    userName=sys.Ini_Read(path+"\\config\\friend.ini","main","account")+"_public";
    
    
    var count=parseInt(strCount);
    for (var i=0;i<count;i++){
        var strName=sys.Ini_Read(path+"\\config\\friend.ini","item"+i,"name");
        //sys.Combox_Add("cb_friend",strName);
    }
    if (count>0){
        //sys.Combox_Select("cb_friend",0);
    }
}

function connect_click(data){
    var url="http://robot6.funnyai.com:8000";
    sys.Socket_Init(url,"event_connected","event_disconnected","event_chat","event_system");
    read_ini();
}

function log_click(data){
    sys.Run_App("D:\\Net\\Web\\log","");
}


function switch_click(data){
    sys.Run_JS("加密聊天_login.js");
}

function chat2(data){
    sys.Run_JS("加密聊天.js");
}

function friend_list(data){
    sys.ListBox_Clear("list_friend");
    sys.ListBox_Add("list_friend","*");
    var strLine="{\"from\":\""+userName+"\",\"type\":\"list.all\",\"to\":\"\",\"message\":\"\"}";
    sys.Send_Msg("sys_event",strLine);
}

function New_URL(data){
    sys.Run_App("chrome.exe", "--incognito "+ data);
}


function callback_login(data){
    read_fav();
}

var arraylist=[];
//读取收藏夹
function read_fav(){
    var file=sys.Value_Read("file");
    if (sys.File_Exists(file)==false){
        return ;
    }
    
    var password=sys.Value_Read("password");
    var strLines=sys.File_Read(file);
    strLines=sys.AES_Decrypt(strLines,password)
    
    strLines=strLines.replaceAll("\n","\r\n");
    sys.Text_Set("edit1",strLines);
}


//保存收藏夹
function save_fav(data){
    var file=sys.Value_Read("file");
    var password=sys.Value_Read("password");
    if (password == "") {
        sys.Msg("密码为空！");
        return;
    }
    
    var strLines = sys.Text_Read("edit1");
    
    strLines=sys.AES_Encrypt(strLines, password);
    sys.File_Save(file,strLines);
    
}

function tab_click(){
    var strLines=sys.Text_Read("edit1");
    
    sys.Text_Set("edit1",strLines+"\t");
}

sys.TextBox_Init("edit1","",10,30,600,390);

sys.Button_Init("b1_send","制表符",520,450,100,70,"tab_click","");


sys.Menu_Init("Menu1",0,0,800,25);
sys.Menu_Add("Menu1","File","&File");
sys.Menu_Item_Add("Menu1","File","Save","保存(&S)","save_fav","");



//其他属性
//sys.Acception_Button("b1_send");
sys.Show_Form(700,600);
sys.Form_Title("加密收藏夹");

read_fav();

