

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
        case "msg":

            if (obj.return_cmd=="matrix"){
                var strSplit=msg.split("=");
                var i=parseInt(strSplit[0]);
                var j=parseInt(strSplit[1]);
                var value=s_math.round(100*parseFloat(strSplit[2]))/100;
                s_ui.datagrid_set("grid1",i,j,value);
                s_ui.datagrid_set("grid1",j,i,value);
            }
            
            switch(obj.return_cmd){
                case "step:1":
                    log_msg=s_time.Time_Now()
                        +" <span style='color:blue;'>"+obj.from+"</span>"
                        +"<br>相关系数="+msg
                        +"<br>"+log_msg;
                    s_ui.Web_Content("web",css_head+log_msg);
                    break;
                default:
                    log_msg=s_time.Time_Now()
                        +" <span style='color:blue;'>"+obj.from+"</span>"
                        +obj.return_cmd+"<br>"+"<pre>"+msg+"</pre>"
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


function check_click(data){
    var index=parseInt(data);
    var k=index+1;
    var a=s_ui.datagrid_read("grid1",index,1);
    //s_ui.msg(a);
    if (a=="1"){
        var file=disk+"\\Net\\Web\\Data\\memo.ini";
        s_file.Ini_Save(file,"y","index",k);
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


function static_click(data){
    var count_field=sys_field_selected();
    
    var line="";
    var line_head="";
    for (var i=0;i<count_field;i++){
        line+="0,";
        line_head+="*,";
    }
    
    
    s_ui.datagrid_clear("grid1");
    s_ui.datagrid_init_column("grid1",count_field,line_head);
    
    
    for (var i=0;i<count_field;i++){
        s_ui.datagrid_add_line("grid1",line,",");
    }
    
    for (var i=0;i<count_field;i++){
        s_ui.datagrid_set("grid1",i,i,"1");
    }
    
    //s_ui.msg("step2");
    var id=s_ui.text_read("model_id");
    var file="/root/happyli/app/pearson_correlation_matrix.js";
    var file_input=s_ui.text_read("txt_file");
    var strMsg="run_js2 "+file+" 0 "+file_input+" "+count_field;
    //s_ui.msg(strMsg);
    send_msg("cmd",friend,strMsg,"step:1");
    //s_ui.text_set("txt_send","");
}


s_ui.datagrid_init("grid1",10,10,600,360);
//界面
s_ui.Web_Init("web",620,10,750,360);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.label_init("lb_alg","要处理的文件:",10,380);

s_ui.text_init("txt_file","/root/data/train.txt",10,420,300,30);


s_ui.button_init("b_correlation","相关性",380,380,100,60,"static_click","");


s_ui.button_init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat2\\step_roc");
//s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat2\\step10");


s_ui.show_form(800,600);
s_ui.Form_Title("v2 相关性");

s_sys.tcp_event();

on_load();

