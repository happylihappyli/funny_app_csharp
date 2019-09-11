
var userName="none";
var log_msg="";

var msg_id=0;
var myMap=[];


function check_myMap() {
    for(var pMsg in myMap){ 
        if (pMsg.Count<3){
            pMsg.Count+=1;
            s_net.Send_Msg("chat_event",pMsg.Msg);
        }else{
            var obj=JSON.parse(pMsg.Msg);
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



function read_ini(){
    //s_ui.Combox_Clear("cb_friend");
    var path=sys.AppPath();
    var strCount=sys.Ini_Read(path+"\\config\\friend.ini","items","count");
    userName=sys.Ini_Read(path+"\\config\\friend.ini","main","account")+"_public";
    
    
    var count=parseInt(strCount);
    for (var i=0;i<count;i++){
        var strName=sys.Ini_Read(path+"\\config\\friend.ini","item"+i,"name");
        //s_ui.Combox_Add("cb_friend",strName);
    }
    if (count>0){
        //s_ui.Combox_Select("cb_friend",0);
    }
}

function login_click(data){
    var pass1 = s_ui.Text_Read("password1");
    var pass2 = s_ui.Text_Read("password2");
    if (pass1!=pass2){
        s_ui.Msg("两次密码不一样！");
        return ;
    }

    sys.Ini_Save("D:\\Net\\Web\\funnyfav.ini","main","file",s_ui.Combox_Text("combox1"));
    
    
    sys.Value_Save("password",pass1);
    sys.Value_Save("file",s_ui.Combox_Text("combox1"));
    
    s_ui.Close();
}
    

s_ui.Label_Init("lb1","输入密码:",10,30);
s_ui.Password_Init("password1","",10,50,300,30);
s_ui.Password_Init("password2","",10,100,300,30);
s_ui.Label_Init("lb2","选择收藏夹文件:",10,180);

s_ui.Combox_Init("combox1","D:\\Net\\Web\\private_url.txt",10,200,300,30);
s_ui.Combox_Add("combox1","D:\\Net\\Web\\private_url.txt");

s_ui.Combox_Add("combox1","E:\\CloudStation\\Robot5\\happyli\\bak\\private_url.txt");

s_ui.Combox_Add("combox1","C:\\Net\\Web\\private_url.txt");

s_ui.Button_Init("b1_login","登录",10,250,100,30,"login_click","");


var a=sys.Ini_Read("D:\\Net\\Web\\funnyfav.ini","main","file");
s_ui.Combox_Text_Set("combox1",a);


//其他属性
s_ui.Acception_Button("b1_login");
s_ui.Show_Form(560,380);
s_ui.Form_Title("登录");
