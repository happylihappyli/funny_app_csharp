
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
    
    var strMsg=s_ui.Text_Read("txt_send");
    var friend=s_ui.ListBox_Text("list_friend");
    var index=s_ui.ListBox_Index("list_friend");
    if (index<0){
        s_ui.Msg("请选择好友！");
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
        
    s_ui.Web_Content("web",log_msg);
    s_ui.Text_Set("txt_send","");
    
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
    sys.Socket_Connect();
}

function clear_click(data){
    log_msg="clear\r\n";
    s_ui.Web_Content("web",log_msg);
}


function event_chat(data){
    var obj=JSON.parse(data);
    var friend=obj.from;
    
    switch (obj.type){
        case "game.wzq":
            var strSplit=obj.message.split(":");
            var x=strSplit[2];
            var y=strSplit[3];
            draw_sub(x,y,"red");
            log_msg=sys.Time_Now()+" "+friend+" &gt; <span style='color:#aaaaaa;'>"+obj.to+"</span> <font color=blue><br>"
            +obj.message+"</font><br><br>\r\n"+log_msg;
            break;
        case "encrypt":
            
            break;
        default:
            var index=s_ui.ListBox_Select("list_friend",friend);
            if (index==-1){
                friend_list("");
                sys.sleep(10);
                s_ui.ListBox_Select("list_friend",friend);
            }
            var strMsg=sys.Time_Now()+" "+obj.message;
            sys.File_Append("D:\\Net\\Web\\log\\"+friend+".txt",sys.Date_Now()+" "+strMsg+"\r\n");
            log_msg=sys.Time_Now()+" "+friend+" &gt; <span style='color:#aaaaaa;'>"+obj.to+"</span> <font color=blue><br>"
            +obj.message+"</font><br><br>\r\n"+log_msg;
            
            var id=obj.id;
            var strLine="{\"from\":\""+userName+"\",\"type\":\"chat_return\",\"to\":\""+obj.from+"\",\"message\":\""+id+"\"}";
            sys.Send_Msg("sys_event",strLine); //服务器会记录用户名
            break;
        
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
                sys.Send_Msg("sys_event",strLine); //服务器会记录用户名
            }
            break;
        case "list.all":
            s_ui.ListBox_Add("list_friend",obj.message);
            break;
    }
}

function read_ini(){
    var path=sys.AppPath();
    var strCount=sys.Ini_Read(path+"\\config\\friend.ini","items","count");
    userName=sys.Ini_Read(path+"\\config\\friend.ini","main","account")+"_public";
    
    var count=parseInt(strCount);
    //s_ui.Combox_Clear("cb_friend");
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
    sys.Socket_Init(url,"event_connected","event_disconnected","event_chat","event_system");
    read_ini();
}

function log_click(data){
    s_ui.Run_App("D:\\Net\\Web\\log","");
}

function friend_list(data){
    s_ui.ListBox_Clear("list_friend");
    s_ui.ListBox_Add("list_friend","*");
    var strLine="{\"from\":\""+userName+"\",\"type\":\"list.all\",\"to\":\"\",\"message\":\"\"}";
    sys.Send_Msg("sys_event",strLine);
}

function draw_test(data){
    s_ui.PictureBox_Draw_Ellipse("pic1",50,50,100,100,"blue",2);
}


function draw_test2(data){
    s_ui.PictureBox_Draw_Ellipse("pic1",100,50,100,100,"blue",2);
}


function mouse_up(arr){
    var index=s_ui.ListBox_Index("list_friend");
    if (index<0){
        s_ui.Msg("请选择好友！");
        return ;
    }
    
    var x=arr[0];
    var y=arr[1];
    x=s_math.round(x/50);
    y=s_math.round(y/50);
    
    draw_sub(x,y,"blue");

    
    var friend=s_ui.ListBox_Text("list_friend");
    var strLine="{\"from\":\""+userName+"\",\"type\":\"game.wzq\",\"to\":\""+friend+"\",\"message\":\"x:y:"+x+":"+y+"\"}";

    sys.Send_Msg("chat_event",strLine);
}

function draw_sub(x,y,color){
    var xx=50*x;
    var yy=50*y;
    
    s_ui.PictureBox_Draw_Ellipse("pic1",xx-20,yy-20,40,40,color,2);
}


function board_init(){
    var max=650;
    for (var i=0;i<13;i++){
        s_ui.PictureBox_Draw_Line("pic1",50,50+50*i,max,50+50*i,"blue",2);
    }
    
    for (var i=0;i<13;i++){
        s_ui.PictureBox_Draw_Line("pic1",50+50*i,50,50+50*i,max,"blue",2);
    }
}

s_ui.Button_Init("btn_friend","刷新好友列表",10,30,250,30,"friend_list","");
s_ui.ListBox_Init("list_friend",10,60,250,80);


s_ui.Button_Init("b_clear","清空聊天记录",10,150,250,30,"clear_click","");

s_ui.Web_Init("web",10,200,250,150);
s_ui.Web_Content("web","接收到信息");

s_ui.Text_Init("txt_send","hi",10,350,250,30);
s_ui.Button_Init("b1_send","发送",10,400,100,30,"send_msg_click","");



s_ui.Text_Init("txt_user_name","000",10,450,100,30);
s_ui.Button_Init("btn_connect","连服务器",120,450,100,30,"connect_click","");
s_ui.Text_Init("txt_session","000",10,500,200,30);



s_ui.TextBox_Init("txt_info","",10,650,250,80);


s_ui.Menu_Init("Menu1",0,0,800,25);
s_ui.Menu_Add("Menu1","File","&File");
s_ui.Menu_Item_Add("Menu1","File","Log","日志(&L)","log_click","");
s_ui.Menu_Item_Add("Menu1","File","Chat2","加密聊天","chat2","");



s_ui.PictureBox_Init("pic1",280,30,900,700);
s_ui.PictureBox_Event("pic1","mouse_up");


s_ui.Button_Init("b2_2","draw1",10,600,100,30,"draw_test","");
s_ui.Button_Init("b2_3","draw2",150,600,100,30,"draw_test2","");

//其他属性
s_ui.Acception_Button("b1_send");
s_ui.Show_Form(1200,800);
s_ui.Form_Title("五子棋");

//s_ui.ShowInTask(0);
connect_click("");
board_init();
