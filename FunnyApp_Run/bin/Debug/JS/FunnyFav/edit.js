
var userName="none";
var log_msg="";

var msg_id=0;
var myMap=[];

[[[..\\data\\common_string.js]]]

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
    //s_ui.Combox_Clear("cb_friend");
    var path=s_sys.path_app();
    var strCount=s_file.Ini_Read(path+"\\config\\friend.ini","items","count");
    userName=s_file.Ini_Read(path+"\\config\\friend.ini","main","account")+"_public";
    
}

function connect_click(data){
    var url="http://robot6.funnyai.com:8000";
    s_net.Socket_Init(url,"event_connected","event_disconnected","event_chat","event_system");
    read_ini();
}

function log_click(data){
    s_ui.Run_App(disk+"\\Net\\Web\\log","");
}


function switch_click(data){
    s_ui.Run_JS("加密聊天_login.js");
}

function chat2(data){
    s_ui.Run_JS("加密聊天.js");
}

function friend_list(data){
    s_ui.listbox_clear("list_friend");
    s_ui.listbox_add("list_friend","*");
    var strLine="{\"from\":\""+userName+"\",\"type\":\"list.all\",\"to\":\"\",\"message\":\"\"}";
    s_net.Send_Msg("sys_event",strLine);
}

function New_URL(data){
    s_ui.Run_App("chrome.exe", "--incognito "+ data);
}


function callback_login(data){
    read_fav();
}

var arraylist=[];
//读取收藏夹
function read_fav(){
    var file=s_sys.value_read("file");
    if (s_file.File_Exists(file)==false){
        return ;
    }
    
    var password=s_sys.value_read("password");
    var strLines=s_file.read(file);
    strLines=s_string.AES_Decrypt(strLines,password)
    
    strLines=strLines.replaceAll("\n","\r\n");
    s_ui.text_set("edit1",strLines);
}


//保存收藏夹
function save_fav(data){
    var file=s_sys.value_read("file");
    var password=s_sys.value_read("password");
    if (password == "") {
        s_ui.msg("密码为空！");
        return;
    }
    
    var strLines = s_ui.text_read("edit1");
    
    strLines=s_string.AES_Encrypt(strLines, password);
    s_file.save(file,strLines);
    
}

function tab_click(){
    var strLines=s_ui.text_read("edit1");
    
    s_ui.text_set("edit1",strLines+"\t");
}

s_ui.textbox_init("edit1","",10,30,600,390);

s_ui.button_init("b1_send","制表符",520,450,100,70,"tab_click","");


s_ui.menu_init("Menu1");//,0,0,800,25);
s_ui.menu_add("Menu1","File","&File");
s_ui.menu_item_add("Menu1","File","Save","保存(&S)","save_fav","");



//其他属性
//s_ui.button_default("b1_send");
s_ui.show_form(700,600);
s_ui.Form_Title("加密收藏夹");

read_fav();

