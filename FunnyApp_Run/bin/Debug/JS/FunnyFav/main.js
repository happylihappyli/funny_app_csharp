
var userName="none";
var log_msg="";

var msg_id=0;
var myMap=[];

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


function event_connected(data){
    s_ui.text_set("txt_info","event_connected");
    s_ui.button_enable("btn_connect",0);
    friend_list();
    
}

function event_disconnected(data){
    s_ui.text_set("txt_info","event_disconnected");
    s_ui.button_enable("btn_connect",1);
    s_net.Socket_Connect();
}

function clear_click(data){
    log_msg="clear\r\n";
    s_ui.Web_Content("web",log_msg);
}


function read_ini(){
    //s_ui.Combox_Clear("cb_friend");
    var path=s_sys.path_app();
    var strCount=s_file.Ini_Read(path+"\\config\\friend.ini","items","count");
    userName=s_file.Ini_Read(path+"\\config\\friend.ini","main","account")+"_public";
    
    
    var count=parseInt(strCount);
    for (var i=0;i<count;i++){
        var strName=s_file.Ini_Read(path+"\\config\\friend.ini","item"+i,"name");
        //s_ui.combox_add("cb_friend",strName);
    }
    if (count>0){
        //s_ui.Combox_Select("cb_friend",0);
    }
}


function log_click(data){
    s_ui.Run_App("D:\\Net\\Web\\log","");
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
    //s_ui.msg("step1");
    read_fav();
}

function callback_edit(data){
    read_fav();
}


var arraylist=[];
//读取收藏夹
function read_fav(){
    //s_ui.msg("step2");
    var file=s_sys.value_read("file");
    if (s_file.exists(file)==false){
        s_ui.msg("no file="+file);
        return ;
    }
    //s_ui.msg("step3");
    arraylist=[];
    //s_ui.msg("step4");
    var password=s_sys.value_read("password");
    //s_ui.msg("step5");
    //s_ui.msg(password);
    var strLines=s_file.read(file);
    //s_ui.msg(strLines);
    strLines=s_string.AES_Decrypt(strLines,password)
    //s_ui.msg(strLines);
    
    strLines=strLines.replace("\r","");
    var strSplit = strLines.split('\n');
    for (var i = 0; i < strSplit.length; i++) {
        var strSplit2 = strSplit[i].split('\t');
        if (strSplit2.length > 1) {
            var name=strSplit2[0];
            var url=strSplit2[1];
            arraylist.push(new C_Link(name,url));
        }
    }
    show_html();
}

function show_html(){
    
    var strHTML="";
    for (var i=0; i<arraylist.length; i++) {
        var pLink=arraylist[i];
        strHTML += "<a href='" + pLink.url + "' target=_blank>"+pLink.name+" "+pLink.url+"</a><br>";
    }
    s_ui.Web_Content("web",strHTML);
}

//保存收藏夹
function save_fav(data){
    var file=s_sys.value_read("file");
    var password=s_sys.value_read("password");
    if (password == "") {
        s_ui.msg("密码为空！");
        return;
    }
    
    var strLines = "";
    for (var i=0; i<arraylist.length; i++) {
        var pLink=arraylist[i];
        strLines += pLink.name + "\t" + pLink.url + "\n";
    }
    //s_ui.msg(strLines);
    
    strLines=s_string.AES_Encrypt(strLines, password);
    s_file.save(file,strLines);
    
}

function add_click(){
    var name=s_ui.text_read("txt_name");
    var url =s_ui.text_read("txt_url");
    arraylist.push(new C_Link(name,url));
    save_fav("");
    show_html();
}

function edit_click(data){
    s_ui.Run_JS_Dialog("FunnyFav\\edit.js","callback_edit");
}



s_ui.Web_Init("web",10,30,600,390);
var a="<a href='http://www.funnyai.com/' target=_blank>funnyai</a>";
s_ui.Web_Content("web",a);
s_ui.Web_New_Event("web","New_URL");



s_ui.button_init("b1_send","添加",520,450,100,70,"add_click","");

s_ui.label_init("lb1","名称：",10,450);

s_ui.text_init("txt_name","",100,450,390,30);


s_ui.label_init("lb2","网址：",10,500);

s_ui.text_init("txt_url","",100,500,390,30);



s_ui.menu_init("Menu1");
s_ui.menu_add("Menu1","File","&File");
s_ui.menu_item_add("Menu1","File","Edit","编辑(&L)","edit_click","");
s_ui.menu_item_add("Menu1","File","Save","保存(&S)","save_fav","");



//其他属性
//s_ui.button_default("b1_send");
s_ui.show_form(700,600);
s_ui.Form_Title("加密收藏夹");

s_ui.Run_JS_Dialog("FunnyFav\\login.js","callback_login");
