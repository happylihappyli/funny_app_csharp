
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
    var strCount=s_file.Ini_Read(path+"\\config\\friend.ini","items","count");
    userName=s_file.Ini_Read(path+"\\config\\friend.ini","main","account")+"_public";
    
    
    var count=parseInt(strCount);
    for (var i=0;i<count;i++){
        var strName=s_file.Ini_Read(path+"\\config\\friend.ini","item"+i,"name");
        //s_ui.Combox_Add("cb_friend",strName);
    }
    if (count>0){
        //s_ui.Combox_Select("cb_friend",0);
    }
}

function save_check_click(data){
    var name = s_ui.Text_Read("name");
    if (name==""){
        s_ui.Msg("请输入用户名");
        return ;
    }
    var password=s_ui.Text_Read("password");
    var md5=s_string.md5(password);
    
    s_file.Ini_Save("D:\\Net\\Web\\main.ini","main","account",name);
    s_file.Ini_Save("D:\\Net\\Web\\main.ini","main","md5",md5);
    
    var url="http://www.funnyai.com/login_check_json.php";
    var data="email="+s_string.urlencode(name)+"&password="+s_string.urlencode(md5);
    
    var result=s_net.http_post(url,data);
    s_ui.Msg(result);
    if (result.indexOf("登录成功")>-1){
        s_ui.Close();
    }else{
    }
}
    
var userName=s_file.Ini_Read("D:\\Net\\Web\\main.ini","main","account");


s_ui.Label_Init("lb1","www.funnyai.com的用户名和密码",10,30);

s_ui.Label_Init("lb2","用户名:",10,100);
s_ui.Text_Init("name",userName,150,100,300,30);
s_ui.Label_Init("lb3","密码:",10,150);
s_ui.Password_Init("password","",150,150,300,30);

s_ui.Button_Init("b1_save","登录",150,200,100,30,"save_check_click","");


//其他属性
s_ui.Acception_Button("b1_save");
s_ui.Show_Form(560,380);
s_ui.Form_Title("参数设置");
