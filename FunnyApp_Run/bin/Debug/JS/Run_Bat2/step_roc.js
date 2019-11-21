
[[[..\\data\\default.js]]]
[[[..\\data\\common_string.js]]]
[[[..\\data\\tcp.js]]]
[[[..\\data\\run_bat_common.js]]]

var file_memo=disk+"\\Net\\Web\\Data\\memo.ini";
var file_ini=disk+"\\Net\\Web\\main.ini";
var friend=s_file.Ini_Read(file_ini,"main","friend_selected");


function on_load(){
    var a=sys_read_ini();
    userName=a+"/linux_bat2";
    //s_ui.text_set("txt_user_name",userName);
    
}

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
        case "file_sql":
            switch(obj.return_cmd){
                case "finished":
                    file_sql
                    break;
            }
        case "msg":
            log_msg=s_time.Time_Now()
                +" <span style='color:blue;'>"+obj.from+"</span>"
                +"<br>"+msg
                +"<br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
            switch(obj.return_cmd){
                case "step:1":
                    process_step2(obj);
                    break;
                case "step:2":
                    process_step3(obj);
                    break;
                case "step:3":
                    process_step4(obj);
                    break;
                default:
                    log_msg=s_time.Time_Now()
                        +"<span style='color:blue;'>"+obj.from+"</span>"
                        +"<br>"+"<pre>"+msg+"</pre>"
                        +"<br>"+log_msg;
                    s_ui.Web_Content("web",css_head+log_msg);
                    break;
            }
            break;
        case "status":
            switch(obj.from){
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


function clear_data(data){
    s_ui.datagrid_clear("grid1");
}

function next_click(data){
    
    s_ui.Run_JS(data+".js");
    s_ui.close();
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
        s_ui.Web_Content("web",css_head+log_msg);
    }else{
        s_ui.status_label_show("status_label","token==null!");
    }
}



function file_sql(file1,sql,sep,output){
    var cmd="file_sql /root/happyli/set_hadoop.ini "+userName+" "+file1
        +" 250000 \""+sql+"\" "+sep+" "+output;
    return cmd;
}


function static_click(data){
    var file1=s_file.Ini_Read(file_memo,"main","file1");
    if (file1=="") file1="E:\\sample1.txt";
    
    var line=s_file.read(file1,1);
    var strSplit=line.split("|");
    var fields_count=strSplit.length;
    
    
    var file2=s_file.Ini_Read(file_memo,"main","file2");
    if (file2=="") file2="/upload/sample1.txt";
    
    var id=s_file.Ini_Read(file_memo,"model","id");
    
    
    var line=s_file.read(file1,1);
    var strSplit=line.split("|");
    var fields_count=strSplit.length;
    var count_field=0;
    
    for (var i=1;i<=fields_count;i++){
        var value=s_file.Ini_Read(file_memo,"selected","check"+i);
        if (value=="1"){
            count_field+=1;
        }
    }
    
    var sql="Select c2,c1 From t;";

    var cmd=file_sql("/root/data/output.txt",sql,",","/root/data/roc_data.txt");
    
    
    send_msg("cmd",friend,cmd,"step:1");
}


function process_step2(obj){
    var cmd=python_bin+" /root/python/draw_roc.py /root/data/roc_data.txt";
    
    send_msg("cmd",friend,cmd,"step:2");
}

function process_step3(obj){
    var cmd="cp -f /root/happyli/Data/roc.png /opt/lampp/htdocs/1.png";
    send_msg("cmd",friend,cmd,"step:3");
}

function process_step4(obj){
    log_msg=s_time.Time_Now()
        +" <span style='color:blue;'>"+obj.from+"</span>"
        +obj.return_cmd+"<br>运行完毕！<a href='http://robot6.funnyai.com/1.png'>1.png</a>"
        +"<br>"+log_msg;
    s_ui.Web_Content("web",css_head+log_msg);
}

//界面
s_ui.Web_Init("web",10,10,750,300);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.progress_init("progress2",100,320,500,30);


s_ui.control_dock("progress2","top");

s_ui.button_init("b_ks","ROC曲线",310,360,200,60,"static_click","");

s_ui.panel_init("panel_bottom",0,0,300,50,"bottom");


s_ui.button_init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat2\\step_ks");
s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat2\\step_correlation");

s_ui.control_dock("web","fill");

s_ui.panel_add("panel_bottom","b_ks","right");
s_ui.panel_add("panel_bottom","b_pre","right");
s_ui.panel_add("panel_bottom","b_next","right");

s_ui.show_form(800,600);
s_ui.Form_Title("v2 ROC曲线");

s_sys.tcp_event();

on_load();

