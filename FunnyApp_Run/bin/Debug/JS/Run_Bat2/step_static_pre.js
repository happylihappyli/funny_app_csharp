
[[[..\\data\\default.js]]]
[[[..\\data\\common_string.js]]]
[[[..\\data\\tcp.js]]]
[[[..\\data\\run_bat_common.js]]]

var file_memo=disk+"\\Net\\Web\\Data\\memo.ini";
var file_ini=disk+"\\Net\\Web\\main.ini";
var friend=s_file.Ini_Read(file_ini,"main","friend_selected");


var friend_return=0;
var session_send=0;
var sep=1;
var step=0;//处理步骤
var row_index=0;//第几个字段被点击
var fields_count=0;

var keep_count=1;

var myMap=[];
var head="";


function event_msg(data){
    
    var obj=JSON.parse(data);
    var msg=obj.message;

    switch(obj.type){
        case "chat_return":
            log_msg="<b>chat_return:"+obj.oid+"</b><br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
            s_ui.status_label_show("status_label2",msg);
            delete myMap["K"+obj.oid];
            break;
        case "msg":
            log_msg=s_time.Time_Now()
                +" <span style='color:blue;'>"+obj.from+"</span>"
                +obj.return_cmd+"<br>"+"<pre>"+data+"</pre>"
                +"<br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
            if (obj.return_cmd=="step:1"){
                //static_bad_click();
            }else if (obj.return_cmd=="step:2"){
                log_msg=s_time.Time_Now()
                    +" <span style='color:blue;'>"+obj.from+"</span>"
                    +"<pre style='color:red;'>运行完毕,点击下一步</pre>"
                    +"<br>"+log_msg;
                s_ui.Web_Content("web",css_head+log_msg);
            }
            break;
        
        case "file_sql":
            if (obj.message=="finished"){
                
                log_msg="<b>finished "+obj.from+";"+step+"</b><br>"+log_msg;
                s_ui.Web_Content("web",css_head+log_msg);
                switch (obj.from){
                    case "/root/data/output_2.txt":
                        process_step(obj);
                        break;
                    case "/root/data/data_for_static.txt":
                        s_ui.Web_Content("web","预处理完毕，点击下一步ks等统计");
                        break;
                }
            }
            break;
        case "status":
            switch(obj.from){
                case "progress1":
                    var strSplit=msg.split(":");
                    s_ui.progress_show("progress1","100",strSplit[0]);
                    s_ui.progress_show("progress2","100","100");
                    break;
                case "progress2":
                    var strSplit=msg.split(":");
                    s_ui.progress_show('progress2', "100",strSplit[0]);
                    break;
            }
            break;
        default:
        
            log_msg="<b>data:"+data+"</b><br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
    
            break;
    }
}



function file_sql(file1,sql,sep,output){
    var cmd="file_sql /root/happyli/set_hadoop.ini "+userName+" "+file1
        +" 250000 \""+sql+"\" "+sep+" "+output;
    return cmd;
}



function static_click(data){
    
    var id=s_file.Ini_Read(file_memo,"model","id");
    
    var count_field=sys_field_selected();
    
    
            
    var sql="select round(c2*100),c1 from t;";

    var cmd=file_sql("/root/data/output.txt",sql,",","/root/data/output_2.txt");
    
    send_msg("cmd",friend,cmd,"step_static_pre:1");
    
}

function process_step(data){
    var sql="select c1,sum(c2),count(1)-sum(c2) From t Group by c1";
    var cmd=file_sql("/root/data/output_2.txt",sql,",","/root/data/data_for_static.txt");
    
    
    send_msg("cmd",friend,cmd,"step_static_pre:2");
}

function next_click(data){
    s_ui.Run_JS(data+".js");
    s_ui.close();
}

function on_load(){
    var a=sys_read_ini();
    userName=a+"/linux_bat2";
    s_ui.text_set("txt_user_name",userName);
    
}

function resend_chat_msg(data) {
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
            s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",
                s_time.Date_Now()+" "+s_time.Time_Now()+" 消息丢失："+obj.message+"\r\n");
        }
    }
}


function set_status(data){
    var strSplit=data.split(",");
    s_ui.progress_show("progress1",strSplit[1],strSplit[0]);
}

var log_error="";
function show_error(data){
    log_error+=data+"\n";
    s_ui.text_set("txt_info",log_error);
}



function send_msg(strType,friend,msg,return_cmd){
    msg_id+=1;
    
    var token=sys_get_token();
    var strLine="";
    
    var strMsg2=msg.replaceAll("\"","\\\"");
    strMsg2=strMsg2.replaceAll("\n","\\n");
    
    if (token!=""){
        strLine="{\"id\":\""+msg_id+"\","
            +"\"token\":\""+token+"\","
            +"\"return_cmd\":\""+return_cmd+"\","
            +"\"from\":\""+userName+"\",\"type\":\""+strType+"\","
            +"\"to\":\""+friend+"\",\"message\":\""+strMsg2+"\"}";
        
        switch(strType){
            case "login":
            case "friend_list":
                break;
            default:
                myMap["K"+msg_id]=new C_Msg(msg_id,strLine);
                break;
        }
        
        s_tcp.send("m:<s>:"+strLine+":</s>");
        
        
        log_msg=s_time.Time_Now()+" 我 &gt; <span style='color:gray;'>"+friend+"</span><br>"
                +msg+"<br><br>"+"<b>tcp.send:"+strLine+"</b>"
                +log_msg;
        s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",
            s_time.Date_Now()+" "+s_time.Time_Now()+" "+msg+"\r\n");
        
        s_ui.Web_Content("web",css_head+log_msg);
    }else{
        s_ui.status_label_show("status_label","token==null!");
    }
}


s_ui.Web_Init("web",250,60,450,250);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.progress_init("progress2",100,400,500,30);

s_ui.control_dock("web","fill");

s_ui.control_dock("progress2","top");



s_ui.panel_init("panel2",0,0,500,50,"bottom");


s_ui.button_init("b_pre", "上一步",100,500,200,30,"next_click","Run_Bat2\\step_lr_test");
s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat2\\step_ks");

s_ui.panel_add("panel2","b_pre","right");
s_ui.panel_add("panel2","b_next","right");


s_ui.status_init("status",0,0,200,30,"bottom");
s_ui.status_label_init("status_label","111",100,30);
s_ui.status_add("status","status_label","left");
s_ui.status_label_init("status_label2","222",100,30);
s_ui.status_add("status","status_label2","left");


s_ui.show_form(800,600);
s_ui.Form_Title("v2 统计准备 ");


s_sys.tcp_event();

on_load("");

static_click("");
