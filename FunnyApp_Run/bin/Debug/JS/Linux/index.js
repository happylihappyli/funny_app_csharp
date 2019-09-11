
var userName="none";
var md5="";
var log_msg="";

var msg_id=0;
var myMap=[];

//消息和发送计数器
function C_Msg(ID,Msg){
    this.ID=ID;
    this.Msg=Msg;
    this.Count=0;
}


//发送消息
function send_msg_click(){
    msg_id+=1;
    
    var index=s_ui.ListBox_Index("list_friend");
    if (index<0){
        s_ui.Msg("请选择好友！");
        return ;
    }
    var strMsg=s_ui.Combox_Text("combox_head")+" "+s_ui.Text_Read("txt_send");
    var friend=s_ui.ListBox_Text("list_friend");
    var strType="js";
    
    var token="";
    
    var url="http://www.funnyai.com/login_get_token_json.php";
    var name=s_sys.Ini_Read("D:\\Net\\Web\\main.ini","main","account");
    var md5=s_sys.Ini_Read("D:\\Net\\Web\\main.ini","main","md5");
    var data="email="+s_string.urlencode(name)+"&password="+s_string.urlencode(md5);
    
    var result=s_net.http_post(url,data);
    if (result.indexOf("登录成功")>-1){
        var strSplit=result.split("=");
        token=strSplit[2];
    }


    var strLine="";
    
    if (token==""){
        strLine="{\"id\":\""+msg_id+"\","
            +"\"from\":\""+userName+"\",\"type\":\""+strType+"\","
            +"\"to\":\""+friend+"\",\"message\":\""+strMsg+"\"}";
    }else{
        strLine="{\"id\":\""+msg_id+"\","
            +"\"token\":\""+token+"\","
            +"\"from\":\""+userName+"\",\"type\":\""+strType+"\","
            +"\"to\":\""+friend+"\",\"message\":\""+strMsg+"\"}";
    }
    
    s_ui.Text_Set("txt_info",strLine);
    
    myMap[msg_id]=new C_Msg(msg_id,strLine);
    
    s_net.Send_Msg("chat_event",strLine);
    
    
    log_msg=s_time.Time_Now()+" 我 &gt; <span style='color:gray;'>"+friend+"</span><br>"
            +strMsg+"<br><br>"+log_msg;
    sys.File_Append("D:\\Net\\Web\\log\\"+friend+".txt",
        s_time.Date_Now()+" "+s_time.Time_Now()+" "+strMsg+"\r\n");
        
    s_ui.Web_Content("web",log_msg);
    s_ui.Text_Set("txt_send","");
    
    sys.setTimeout("check_myMap", 3);//检查消息是否都发送过去了，没有发送的，再发送一次。
    
}

function check_myMap(data) {
    for(var key in myMap){
        var pMsg=myMap[key];
        if (pMsg.Count<3){
            pMsg.Count+=1;
            s_net.Send_Msg("chat_event",pMsg.Msg);
        }else{
            var obj=JSON.parse(pMsg.Msg);
            var friend=pMsg.to;
            log_msg=s_time.Time_Now()+" <font color=red>(消息没有发送) </font> <span style='color:gray;'>"+obj.to+"</span><br>"
                    +obj.message+"<br><br>"+log_msg;
            sys.File_Append("D:\\Net\\Web\\log\\"+friend+".txt",
                s_time.Date_Now()+" "+s_time.Time_Now()+" 消息丢失："+obj.message+"\r\n");
                
            s_ui.Web_Content("web",log_msg);
        }
    }
}

function text_keydown(data){
    if (data==13){
        send_msg_click();
    }
}


function event_connected(data){
    s_ui.Text_Set("txt_info","event_connected");
    s_ui.Button_Enable("btn_connect",0);
    friend_list();
    
}

function event_disconnected(data){
    s_ui.Text_Set("txt_info","event_disconnected");
    s_ui.Button_Enable("btn_connect",1);
    s_net.Socket_Connect();
}

function clear_click(data){
    log_msg="clear\r\n";
    s_ui.Web_Content("web",log_msg);
}


function event_chat(data){
    var obj=JSON.parse(data);
    var friend=obj.from;
    
    if (obj.type=="encrypt"){
    }else{
        var index=s_ui.ListBox_Select("list_friend",friend);
        if (index==-1){
            friend_list("");
            //sys.sleep(10);
            s_ui.ListBox_Select("list_friend",friend);
        }
        var strMsg=s_time.Time_Now()+"<br>"+obj.message;
        sys.File_Append("D:\\Net\\Web\\log\\"+friend+".txt",s_time.Date_Now()+" "+strMsg+"\r\n");
        log_msg=s_time.Time_Now()+" "+friend+" &gt; <span style='color:#aaaaaa;'>"+obj.to+"</span>"
        +"<br><div><textarea style='width:100%;height:600px;'>"
        +obj.message+"</textarea></div><br><br>\r\n"+log_msg;
        
        var id=obj.id;
        var strLine="{\"from\":\""+userName+"\",\"type\":\"chat_return\",\"to\":\""+obj.from+"\",\"message\":\""+id+"\"}";
        s_net.Send_Msg("sys_event",strLine); //消息返回
        
    }
    
    s_ui.Web_Content("web",log_msg);
    
    //s_ui.Notification(obj.from,strMsg);
}

function event_system(data){
    var obj=JSON.parse(data);
    var log_msg2=obj.from+"："+obj.message+"\r\n";
    s_ui.Text_Set("txt_info",log_msg2);
    switch(obj.type){
        case "chat_return":
            s_ui.Text_Set("txt_info","chat_return:"+obj.message);
            delete myMap[obj.message];
            break;
        case "30s:session":
            if (obj.from=="system"){
                session_id=obj.message;
                s_ui.Text_Set("txt_session",session_id);  
                s_ui.Text_Set("txt_user_name",userName);
                var friend="*";
                var strLine="{\"from\":\""+userName+"\",\"type\":\"session\",\"to\":\".\",\"message\":\""+session_id+"\"}";
                s_net.Send_Msg("sys_event",strLine); //服务器会记录用户名
            }
            break;
        case "list.all":
            s_ui.ListBox_Add("list_friend",obj.message);
            break;
    }
}

function read_ini(){
    //s_ui.Combox_Clear("cb_friend");
    var path=sys.AppPath();
    var strCount=sys.Ini_Read(path+"\\config\\friend.ini","items","count");
    
    var userName2=sys.Ini_Read("D:\\Net\\Web\\main.ini","main","account");
    md5=sys.Ini_Read("D:\\Net\\Web\\main.ini","main","md5");
    userName=userName2+"/linux";
    
    
    var count=parseInt(strCount);
    for (var i=0;i<count;i++){
        var strName=sys.Ini_Read(path+"\\config\\friend.ini","item"+i,"name");
        //s_ui.Combox_Add("cb_friend",strName);
    }
    if (count>0){
        //s_ui.Combox_Select("cb_friend",0);
    }
}

function connect_click(data){
    var url="http://robot6.funnyai.com:8000";
    s_net.Socket_Init(url,"event_connected","event_disconnected","event_chat","event_system");
    read_ini();
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
    s_ui.ListBox_Clear("list_friend");
    s_ui.ListBox_Add("list_friend","*");
    var strLine="{\"from\":\""+userName+"\",\"type\":\"list.all\",\"to\":\"\",\"message\":\"\"}";
    s_net.Send_Msg("sys_event",strLine);
}

function set_click(data){
    s_ui.Run_JS("Chat\\setting.js");
}

function cmd(data){
    s_ui.Text_Set("txt_send",data);
    send_msg_click();
}

s_ui.SplitContainer_Init("split",0,0,500,500,"v");

s_ui.Button_Init("btn_friend","刷新好友列表",10,30,200,30,"friend_list","");
s_ui.ListBox_Init("list_friend",10,60,200,180);


s_ui.Button_Init("b_clear","清空聊天记录",250,30,450,30,"clear_click","");

s_ui.Web_Init("web",250,60,450,250);
s_ui.Web_Content("web","接收到信息");



s_ui.Combox_Init("combox_head","/root/test.js",250,350,100,30);
s_ui.Combox_Add("combox_head","/root/test.js");

s_ui.Text_Init("txt_send","ls",380,350,320,30);


s_ui.Button_Init("b1_send","发送",600,400,100,30,"send_msg_click","");



s_ui.Text_Init("txt_user_name","000",10,450,100,30);
s_ui.Button_Init("btn_connect","连服务器",120,450,90,30,"connect_click","");
s_ui.Text_Init("txt_session","000",10,500,200,30);



s_ui.TextBox_Init("txt_info","",10,250,200,80);


s_ui.SplitContainer_Add("split",0,"list_friend","fill");
s_ui.SplitContainer_Add("split",0,"btn_friend","top");

s_ui.SplitContainer_Add("split",0,"txt_info","bottom");
s_ui.SplitContainer_Add("split",0,"txt_user_name","bottom");
s_ui.SplitContainer_Add("split",0,"btn_connect","bottom");
s_ui.SplitContainer_Add("split",0,"txt_session","bottom");


s_ui.SplitContainer_Add("split",0,"combox_head","bottom");



s_ui.SplitContainer_Add("split",1,"web","fill");
s_ui.SplitContainer_Add("split",1,"b_clear","top");



s_ui.Panel_Init("panel_top",0,0,500,25);
s_ui.SplitContainer_Add("split",1,"panel_top","bottom");
s_ui.Panel_Add("panel_top","txt_send","fill");

s_ui.Panel_Add("panel_top","b1_send","right");


s_ui.Panel_Init("panel2",0,0,500,25);
s_ui.SplitContainer_Add("split",1,"panel2","bottom");

s_ui.Button_Init("btn_ls","目录文件",10,30,100,30,"cmd","ls");
s_ui.Panel_Add("panel2","btn_ls","left");
s_ui.Button_Init("btn_ps","查看进程",10,30,100,30,"cmd","ps aux");
s_ui.Panel_Add("panel2","btn_ps","left");
s_ui.Button_Init("btn_ps_js","JS进程",10,30,100,30,"cmd","ps aux|grep .js");
s_ui.Panel_Add("panel2","btn_ps_js","left");
s_ui.Button_Init("btn_ps_java","Java进程",10,30,100,30,"cmd","ps aux|grep java");
s_ui.Panel_Add("panel2","btn_ps_java","left");


s_ui.Menu_Init("Menu1",0,0,800,25);
s_ui.Menu_Add("Menu1","File","&File");
s_ui.Menu_Item_Add("Menu1","File","Log","日志(&L)","log_click","");
s_ui.Menu_Item_Add("Menu1","File","Chat2","加密聊天","chat2","");
s_ui.Menu_Add("Menu1","Tools","&Tools");
s_ui.Menu_Item_Add("Menu1","Tools","Setting","设置(&S)","set_click","");



//其他属性
s_ui.Acception_Button("b1_send");
s_ui.Show_Form(800,600);
s_ui.Form_Title("Linux");

//s_ui.ShowInTask(0);
connect_click("");
