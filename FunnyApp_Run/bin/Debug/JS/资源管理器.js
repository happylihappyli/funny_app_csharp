
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


function event_chat(data){
    var obj=JSON.parse(data);
    var friend=obj.from;
    
    if (obj.type=="encrypt"){
    }else{
        var index=sys.ListBox_Select("list_friend",friend);
        if (index==-1){
            friend_list("");
            sys.sleep(10);
            sys.ListBox_Select("list_friend",friend);
        }
        var strMsg=sys.Time_Now()+" "+obj.message;
        sys.File_Append("D:\\Net\\Web\\log\\"+friend+".txt",sys.Date_Now()+" "+strMsg+"\r\n");
        log_msg=sys.Time_Now()+" "+friend+" &gt; <span style='color:#aaaaaa;'>"+obj.to+"</span> <font color=blue><br>"
        +obj.message+"</font><br><br>\r\n"+log_msg;
        
        var id=obj.id;
        var strLine="{\"from\":\""+userName+"\",\"type\":\"chat_return\",\"to\":\""+obj.from+"\",\"message\":\""+id+"\"}";
        sys.Send_Msg("sys_event",strLine); //服务器会记录用户名
        
    }
    
    sys.Web_Content("web",log_msg);
    
    sys.Notification(obj.from,strMsg);
}

function event_system(data){
    var obj=JSON.parse(data);
    var log_msg2=obj.from+"："+obj.message+"\r\n";
    sys.Show_Text("txt_info",log_msg2);
    switch(obj.type){
        case "chat_return":
            sys.Show_Text("txt_info","chat_return:"+obj.message);
            delete myMap[obj.message];
            break;
        case "30s:session":
            if (obj.from=="system"){
                session_id=obj.message;
                sys.Show_Text("txt_session",session_id);  
                sys.Show_Text("txt_user_name",userName);
                var friend="*";
                var strLine="{\"from\":\""+userName+"\",\"type\":\"session\",\"to\":\".\",\"message\":\""+session_id+"\"}";
                sys.Send_Msg("sys_event",strLine); //服务器会记录用户名
            }
            break;
        case "list.all":
            sys.ListBox_Add("list_friend",obj.message);
            break;
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

function Node_Click(data){
    
}


sys.Tree_Init("tree1",10,60,200,380);

sys.Tree_Add_Node_Root("tree1","root","我的电脑","Node_Click");
sys.Tree_Add_Node("tree1","root","test","test","Node_Click");
sys.Tree_Add_Node_Root("tree1","C","C:","Node_Click");
sys.Tree_Add_Node_Root("tree1","D","D:","Node_Click");
sys.Tree_Add_Node_Root("tree1","E","E:","Node_Click");

sys.Web_Init("web",250,60,450,380);
sys.Web_Content("web","文件");





sys.Menu_Init("Menu1",0,0,800,25);
sys.Menu_Add("Menu1","File","&File");
sys.Menu_Item_Add("Menu1","File","Log","日志(&L)","log_click","");
sys.Menu_Item_Add("Menu1","File","Chat2","加密聊天","chat2","");



//其他属性
sys.Acception_Button("b1_send");
sys.Show_Form(800,600);
sys.Form_Title("资源管理器");

