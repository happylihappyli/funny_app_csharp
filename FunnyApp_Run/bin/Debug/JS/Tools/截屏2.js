
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
    
    var strMsg=s_ui.text_read("txt_send");
    var friend=s_ui.listbox_text("list_friend");
    var index=s_ui.listbox_index("list_friend");
    if (index<0){
        s_ui.msg("请选择好友！");
        return ;
    }
    
    var strLine="{\"id\":\""+msg_id+"\",\"from\":\""+userName+"\",\"type\":\"\",\"to\":\""
    +friend+"\",\"message\":\""+strMsg+"\"}";
    myMap[msg_id]=new C_Msg(msg_id,strLine);
    
    s_net.Send_Msg("chat_event",strLine);
    
    
    log_msg=s_time.Time_Now()+" 我 &gt; <span style='color:gray;'>"+friend+"</span><br>"
            +strMsg+"<br><br>"+log_msg;
    s_file.append("D:\\Net\\Web\\log\\"+friend+".txt",
        s_time.Date_Now()+" "+s_time.Time_Now()+" "+strMsg+"\r\n");
        
    s_ui.Web_Content("web",log_msg);
    s_ui.text_set("txt_send","");
    
    s_time.setTimeout("check_myMap", 3);//检查消息是否都发送过去了，没有发送的，再发送一次。
    
}

function check_myMap() {
    for(var pMsg in myMap){ 
        if (pMsg.Count<3){
            pMsg.Count+=1;
            s_net.Send_Msg("chat_event",pMsg.Msg);
        }else{
            var obj=JSON.parse(pMsg.Msg);
            log_msg=s_time.Time_Now()+" <font color=red>(消息没有发送) </font> <span style='color:gray;'>"+obj.to+"</span><br>"
                    +obj.message+"<br><br>"+log_msg;
            s_file.append("D:\\Net\\Web\\log\\"+friend+".txt",
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


function event_chat(data){
    var obj=JSON.parse(data);
    var friend=obj.from;
    
    switch (obj.type){
        case "game.wzq":
            var strSplit=obj.message.split(":");
            var x=strSplit[2];
            var y=strSplit[3];
            draw_sub(x,y,"red");
            log_msg=s_time.Time_Now()+" "+friend+" &gt; <span style='color:#aaaaaa;'>"+obj.to+"</span> <font color=blue><br>"
            +obj.message+"</font><br><br>\r\n"+log_msg;
            break;
        case "encrypt":
            
            break;
        default:
            var index=s_ui.listbox_select("list_friend",friend);
            if (index==-1){
                friend_list("");
                sys.sleep(10);
                s_ui.listbox_select("list_friend",friend);
            }
            var strMsg=s_time.Time_Now()+" "+obj.message;
            s_file.append("D:\\Net\\Web\\log\\"+friend+".txt",s_time.Date_Now()+" "+strMsg+"\r\n");
            log_msg=s_time.Time_Now()+" "+friend+" &gt; <span style='color:#aaaaaa;'>"+obj.to+"</span> <font color=blue><br>"
            +obj.message+"</font><br><br>\r\n"+log_msg;
            
            var id=obj.id;
            var strLine="{\"from\":\""+userName+"\",\"type\":\"chat_return\",\"to\":\""+obj.from+"\",\"message\":\""+id+"\"}";
            s_net.Send_Msg("sys_event",strLine); //服务器会记录用户名
            break;
        
    }
    
    s_ui.Web_Content("web",log_msg);
    
    //s_ui.Notification(obj.from,strMsg);
}

function event_system(data){
    var obj=JSON.parse(data);
    var log_msg2=obj.from+"："+obj.message+"\r\n";
    s_ui.text_set("txt_info",log_msg2);
    switch(obj.type){
        case "chat_return":
            s_ui.text_set("txt_info","chat_return:"+obj.message);
            delete myMap[obj.message];
            break;
        case "30s:session":
            if (obj.from=="system"){
                session_id=obj.message;
                s_ui.text_set("txt_session",session_id);  
                s_ui.text_set("txt_user_name",userName);
                var friend="*";
                var strLine="{\"from\":\""+userName+"\",\"type\":\"session\",\"to\":\".\",\"message\":\""+session_id+"\"}";
                s_net.Send_Msg("sys_event",strLine); //服务器会记录用户名
            }
            break;
        case "list.all":
            s_ui.listbox_add("list_friend",obj.message);
            break;
    }
}

function read_ini(){
    var path=s_sys.path_app();
    var strCount=s_file.Ini_Read(path+"\\config\\friend.ini","items","count");
    
    var userName2=s_file.Ini_Read("D:\\Net\\Web\\main.ini","main","account");
    userName=userName2+"/public";
    
}

function connect_click(data){
    var url="http://robot6.funnyai.com:8000";
    s_net.Socket_Init(url,"event_connected","event_disconnected","event_chat","event_system");
    read_ini();
}

function log_click(data){
    s_ui.Run_App("D:\\Net\\Web\\log","");
}

function friend_list(data){
    s_ui.listbox_clear("list_friend");
    s_ui.listbox_add("list_friend","*");
    var strLine="{\"from\":\""+userName+"\",\"type\":\"list.all\",\"to\":\"\",\"message\":\"\"}";
    s_net.Send_Msg("sys_event",strLine);
}

function draw_test(data){
    s_ui.picturebox_draw_ellipse("pic1",50,50,100,100,"blue",2);
}


function draw_test2(data){
    s_ui.picturebox_draw_ellipse("pic1",100,50,100,100,"blue",2);
}


function draw_sub(x,y,color){
    var xx=50*x;
    var yy=50*y;
    
    s_ui.picturebox_draw_ellipse("pic1",xx-20,yy-20,40,40,color,2);
}

function set_click(data){
    s_ui.Run_JS("Chat\\setting.js");
}

function callback_capture(){
    s_ui.picturebox_capture_screen("pic1");
    s_ui.win_active();//激活窗口
}

function capture_click(data){
    
    s_ui.win_min();//最小化窗口
    
    s_sys.value_save("sleep",data);
    s_sys.call_thread("截屏.sub.capture.js","callback_capture");
}


function exit_click(data){
    s_ui.close();
}


var w=s_ui.screen_width();
var h=s_ui.screen_height();

s_ui.picturebox_init("pic1",0,0,w,h);
//s_ui.picturebox_event("pic1","mouse_up");

s_ui.control_dock("pic1","fill");

s_ui.menu2_init("popmenu");
s_ui.menu2_add("popmenu","Menu_Exit","退出","exit_click","");
s_ui.menu2_add("popmenu","Menu_Capture","截屏","capture_click","");
s_ui.menu2_add("popmenu","Menu_Capture2","截屏*3秒后","capture_click","3");


s_ui.menu2_container("popmenu","pic1")



s_ui.control_box_set(0);
//其他属性
//s_ui.button_default("b_capture");
s_ui.show_form_pos(0,0,1100,800);
s_ui.Form_Title("");

//s_ui.ShowInTask(0);
connect_click("");

