var userName="test";
var disk="D:";

[[[..\\data\\common_string.js]]]

var msg_id=1;


function read_ini(){
    //s_ui.Combox_Clear("cb_friend");
    var path=s_sys.AppPath();
    var strCount=s_file.Ini_Read(path+"\\config\\friend.ini","items","count");
    
    var userName2=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","account");
    md5=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","md5");
    userName=userName2+"/tcp";
    
}

function send_msg_click(data){
    var msg=s_ui.text_read("send_msg")
    
    var url="http://www.funnyai.com/login_get_token_json.php";
    var name=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","account");
    var md5=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","md5");
    var data="email="+s_string.urlencode(name)+"&password="+s_string.urlencode(md5);
    
    var result=s_net.http_post(url,data);
    if (result.indexOf("登录成功")>-1){
        var strSplit=result.split("=");
        token=strSplit[2];
    }
    
    var strType="cmd";
    var strLine="";
    var friend="*";
    
    var strMsg2=msg.replaceAll("\"","\\\"");
    
    if (token==""){
        strLine="{\"id\":\""+msg_id+"\","
            +"\"from\":\""+userName+"\",\"type\":\""+strType+"\","
            +"\"to\":\""+friend+"\",\"message\":\""+strMsg2+"\"}";
    }else{
        strLine="{\"id\":\""+msg_id+"\","
            +"\"token\":\""+token+"\","
            +"\"from\":\""+userName+"\",\"type\":\""+strType+"\","
            +"\"to\":\""+friend+"\",\"message\":\""+strMsg2+"\"}";
    }
    
    s_tcp.send("m:<s>:"+strLine+":</s>");
}


var log_msg="";
var keep_count=1;
function event_msg(data){
    data=data.replaceAll("\r\n","\n");
    var strSplit=data.split("\n");
    if (strSplit[0]=="s:keep"){
        s_ui.text_set("tx_status","keep"+keep_count);
        keep_count++;
    }else if(strSplit[0].indexOf("m:<s>:")==0){
        show_msg(data);
    }else{
        log_msg=data+"\r\n"+log_msg;
        s_ui.text_set("txt1",log_msg);
    }
}

function show_msg(data){
    var index1=data.indexOf(":<s>:");
    var index2=data.indexOf(":</s>");
    if (index2>index1 && index1>0){
        while(index2>index1 && index1>0){
            var json=data.substring(index1+5,index2);
            var obj=JSON.parse(json);
            var msg=obj.message;
            //s_ui.msg(obj.type);
            switch(obj.type){
                case "chat_return":
                    s_ui.text_set("tx_status",msg);
                    break;
                default:
                    //s_ui.msg(msg);
                    msg=msg.replaceAll("\n","\r\n");
                    log_msg=msg+"\r\n"+log_msg;
                    s_ui.text_set("txt1",log_msg);
                    break;
            }
            data=data.substring(index2+6);
            
            index1=data.indexOf(":<s>:");
            index2=data.indexOf(":</s>");
        }
    }else{
        log_msg=index1+":"+index2+":";//+data+"\r\n"+log_msg;
        s_ui.text_set("txt1",log_msg);
    }
}

function connect_click(data){
    
    s_tcp.connect("robot6.funnyai.com",6000,userName,"event_msg");
}

function clear_click(data){
    log_msg="";
    s_ui.text_set("txt1",log_msg);
    
}

s_ui.button_init("b_send","发送",250,50,100,30,"send_msg_click","");

s_ui.text_init("send_msg","ls",10,50,200,50);

s_ui.text_init("tx_status","",10,100,200,50);
s_ui.button_init("b_connect","连接",250,100,100,30,"connect_click","");
s_ui.button_init("b_clear","Clear",350,100,100,30,"clear_click","");

s_ui.textbox_init("txt1","显示信息：",10,150,500,200);


s_ui.button_default("b_send");

s_ui.Show_Form(600,500);
s_ui.Form_Title("TCP");

read_ini("");



