
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

//发送消息
function send_msg_click(){
    msg_id+=1;
    
    var strMsg=sys.Get_Text("txt_send");
    var friend=sys.ListBox_Text("list_friend");
    var index=sys.ListBox_Index("list_friend");
    if (index<0){
        sys.Msg("请选择好友！");
        return ;
    }
    
    var strLine="{\"id\":\""+msg_id+"\",\"from\":\""+userName+"\",\"type\":\"\",\"to\":\""
    +friend+"\",\"message\":\""+strMsg+"\"}";
    myMap[msg_id]=new C_Msg(msg_id,strLine);
    
    sys.Send_Msg("chat_event",strLine);
    
    
    log_msg=sys.Time_Now()+" 我 &gt; <span style='color:gray;'>"+friend+"</span><br>"
            +strMsg+"<br><br>"+log_msg;
    sys.File_Append("D:\\Net\\Web\\log\\"+friend+".txt",
        sys.Date_Now()+" "+sys.Time_Now()+" "+strMsg+"\r\n");
        
    sys.Web_Content("web",log_msg);
    sys.Show_Text("txt_send","");
    
    sys.setTimeout("check_myMap", 3);//检查消息是否都发送过去了，没有发送的，再发送一次。
    
}

function check_myMap() {
    for(var pMsg in myMap){ 
        if (pMsg.Count<3){
            pMsg.Count+=1;
            sys.Send_Msg("chat_event",pMsg.Msg);
        }else{
            var obj=JSON.parse(pMsg.Msg);
            log_msg=sys.Time_Now()+" <font color=red>(消息没有发送) </font> <span style='color:gray;'>"+obj.to+"</span><br>"
                    +obj.message+"<br><br>"+log_msg;
            sys.File_Append("D:\\Net\\Web\\log\\"+friend+".txt",
                sys.Date_Now()+" "+sys.Time_Now()+" 消息丢失："+obj.message+"\r\n");
                
            sys.Web_Content("web",log_msg);
        }
    }
}

function text_keydown(data){
    if (data==13){
        send_msg_click();
    }
}


function event_connected(data){
    sys.Show_Text("txt_info","event_connected");
    sys.Button_Enable("btn_connect",0);
    friend_list();
    
}

function event_disconnected(data){
    sys.Show_Text("txt_info","event_disconnected");
    sys.Button_Enable("btn_connect",1);
    sys.Socket_Connect();
}

function clear_click(data){
    log_msg="clear\r\n";
    sys.Web_Content("web",log_msg);
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

function callback_edit(data){
    read_fav();
}


var arraylist=[];
//读取收藏夹
function read_fav(){
    var file=sys.Value_Read("file");
    if (sys.File_Exists(file)==false){
        return ;
    }
    arraylist=[];
    var password=sys.Value_Read("password");
    var strLines=sys.File_Read(file);
    strLines=sys.AES_Decrypt(strLines,password)
    
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
    sys.Web_Content("web",strHTML);
}

//保存收藏夹
function save_fav(data){
    var file=sys.Value_Read("file");
    var password=sys.Value_Read("password");
    if (password == "") {
        sys.Msg("密码为空！");
        return;
    }
    
    var strLines = "";
    for (var i=0; i<arraylist.length; i++) {
        var pLink=arraylist[i];
        strLines += pLink.name + "\t" + pLink.url + "\n";
    }
    //sys.Msg(strLines);
    
    strLines=sys.AES_Encrypt(strLines, password);
    sys.File_Save(file,strLines);
    
}

function add_click(){
    var name=sys.Get_Text("txt_name");
    var url =sys.Get_Text("txt_url");
    arraylist.push(new C_Link(name,url));
    save_fav("");
    show_html();
}

function edit_click(data){
    sys.Run_JS_Dialog("FunnyFav/edit.js","callback_edit");
}



sys.Web_Init("web",10,30,600,390);
var a="<a href='http://www.funnyai.com/' target=_blank>funnyai</a>";
sys.Web_Content("web",a);
sys.Web_New_Event("web","New_URL");

sys.Button_Init("b1_send","添加",520,450,100,70,"add_click","");

sys.Label_Init("lb1","名称：",10,450);
sys.Text_Init("txt_name","",100,450,390,30);
sys.Label_Init("lb1","网址：",10,500);
sys.Text_Init("txt_url","",100,500,390,30);



sys.Menu_Init("Menu1",0,0,800,25);
sys.Menu_Add("Menu1","File","&File");
sys.Menu_Item_Add("Menu1","File","Edit","编辑(&L)","edit_click","");
sys.Menu_Item_Add("Menu1","File","Save","保存(&S)","save_fav","");



//其他属性
//sys.Acception_Button("b1_send");
sys.Show_Form(700,600);
sys.Form_Title("加密收藏夹");

sys.Run_JS_Dialog("FunnyFav/login.js","callback_login");
