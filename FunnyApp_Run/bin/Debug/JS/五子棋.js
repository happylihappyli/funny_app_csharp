
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

function draw_test(data){
    sys.PictureBox_Draw_Ellipse("pic1",50,50,100,100,"blue",2);
}

function draw_test2(data){
    sys.PictureBox_Draw_Ellipse("pic1",100,50,100,100,"blue",2);
}

function mouse_up(arr){
    var x=arr[0];
    var y=arr[1];
    sys.PictureBox_Draw_Ellipse("pic1",x-50,y-50,100,100,"red",2);
}


sys.Button_Init("btn_friend","刷新好友列表",10,30,200,30,"friend_list","");
sys.ListBox_Init("list_friend",10,60,200,380);


sys.Button_Init("b_clear","清空聊天记录",250,30,250,30,"clear_click","");

sys.Web_Init("web",250,60,250,250);
sys.Web_Content("web","接收到信息");

sys.Add_Text("txt_send","hi",250,350,250,30);
sys.Button_Init("b1_send","发送",400,400,100,30,"send_msg_click","");



sys.Add_Text("txt_user_name","000",10,450,100,30);
sys.Button_Init("btn_connect","连服务器",120,450,100,30,"connect_click","");
sys.Add_Text("txt_session","000",10,500,200,30);



sys.TextBox_Init("txt_info","",250,450,250,80);


sys.Menu_Init("Menu1",0,0,800,25);
sys.Menu_Add("Menu1","File","&File");
sys.Menu_Item_Add("Menu1","File","Log","日志(&L)","log_click","");
sys.Menu_Item_Add("Menu1","File","Chat2","加密聊天","chat2","");



sys.PictureBox_Init("pic1",550,30,600,600);
sys.PictureBox_Event("pic1","mouse_up");


sys.Button_Init("b2_2","draw1",250,600,100,30,"draw_test","");
sys.Button_Init("b2_3","draw2",400,600,100,30,"draw_test2","");

//其他属性
sys.Acception_Button("b1_send");
sys.Show_Form(1200,800);
sys.Form_Title("聊天");

//sys.ShowInTask(0);
connect_click("");

