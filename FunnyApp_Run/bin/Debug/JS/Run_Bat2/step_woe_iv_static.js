var friend_return=0;
var session_send=0;
var sep=1;
var step=0;//处理步骤
var row_index=0;//第几个字段被点击
var fields_count=0;

var keep_count=1;

var myMap=[];
var head="";

[[[..\\data\\default.js]]]
[[[..\\data\\common_string.js]]]
[[[..\\data\\tcp.js]]]
[[[..\\data\\run_bat_common.js]]]



var file_memo=disk+"\\Net\\Web\\Data\\memo.ini";
var file_ini=disk+"\\Net\\Web\\main.ini";
var friend=s_file.Ini_Read(file_ini,"main","friend_selected");

function clear_data(data){
    s_ui.datagrid_clear("grid1");
}

function data_init(data){
    s_ui.datagrid_clear("grid1");
    
    s_ui.datagrid_init_column("grid1",2,"字段,");
    s_ui.datagrid_add_line("grid1","1,选择要统计的字段...",",");
    
    s_ui.datagrid_add_checkbox("grid1","modify","选择","check_click");
    //s_ui.datagrid_add_button("grid1","modify","映射","map_click");
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
            if (obj.message=="finished"){
                
                log_msg="<b>finished "+obj.from+";"+step+"</b><br>"+log_msg;
                s_ui.Web_Content("web",css_head+log_msg);
                switch (obj.from){
                    case "/root/static_output.txt":
                        process_step12(obj);
                        break;
                }
            }
            break;
        case "msg":
            if (obj.return_cmd=="step:12"){
                //log_msg="<b>finished "+obj.from+";"+data+"</b><br>"+log_msg;
                //s_ui.Web_Content("web",css_head+log_msg);
                process_step13(msg);
            }
            
            break;
        case "status":
            switch(obj.from){
                case "progress1":
                    var strSplit=msg.split(":");
                    //s_ui.progress_show("progress1","100",strSplit[0]);
                    //s_ui.progress_show("progress2","100","100");
                    break;
                case "progress2":
                    var strSplit=msg.split(":");
                    s_ui.progress_show('progress2', "100",strSplit[0]);
                    break;
            }
            break;
        default:
            log_msg=s_time.Time_Now()
                +"<span style='color:red;'>"+obj.from+"</span><br>"
                +msg+"<br><br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
            break;
    }
}


    
function process_step11(data){
    var file1="/root/step6.txt";
    var index=parseInt(data)+1;
    
    var y_index=s_file.Ini_Read(file_memo,"y","index");
    var count_field=sys_field_selected();
    var sql="select c"+index+",sum(c"+y_index+"),count(1)-sum(c"+y_index+")"
        +" From t Group By c"+index;
    var output="/root/static_output.txt";
    var cmd=file_sql(file1,sql,",",output);
    send_msg("cmd",friend,cmd,"step:11");
}


function process_step12(data){
    var cmd="cat /root/static_output.txt";
    send_msg("cmd",friend,cmd,"step:12");
}



function WOE_IV(data){
    
    var count=s_ui.datagrid_rows("grid1")-1;

    var a=new Array(count);
    var b=new Array(count);
    var data=new Array(count);
    
    var sum_a=0;
    var sum_b=0;
    for (var i=0;i<count;i++){
        data[i]= new Array(2);
        data[i][0]=parseFloat(s_ui.datagrid_read("grid1",i,1));
        data[i][1]=parseFloat(s_ui.datagrid_read("grid1",i,2));
        a[i]=data[i][0];
        b[i]=data[i][1];
        
        sum_a+=a[i];
        sum_b+=b[i];
    }
    
    var IV=0;
    for (var i=0;i<count;i++){
        var x=(a[i]/sum_a)/(b[i]/sum_b);
        var woe=s_math.ln(x);
        s_ui.datagrid_set("grid1",i,3,woe);
        
        IV+=((a[i]/sum_a)-(b[i]/sum_b))*woe;
    }
    
    
    s_ui.datagrid_set("grid1",0,4,IV);
        
    var strInfo="<br> &lt; 0.03 无预测能力<br>"
            +"0.03 - 0.09 低<br>"
            +"0.1 - 0.29 中<br>"
            +"0.3 - 0.49 高<br>"
            +"&gt;=0.5 极高<br>";
    //s_ui.msg(strInfo);
    s_ui.Web_Content("web","IV="+IV+strInfo);
}


var b_step13=0;
function process_step13(data){
    if (b_step13==0){
        var strSplit=data.split("\n");
        for (var i=0;i<strSplit.length;i++){
            if (strSplit[i]!=""){
                s_ui.datagrid_add_line("grid1",strSplit[i],",");
            }
        }
        b_step13=1;
        WOE_IV("");
    }
}

function file_sql(file1,sql,sep,output){
    var cmd="file_sql /root/happyli/set_hadoop.ini "+userName+" "+file1
        +" 250000 \""+sql+"\" "+sep+" "+output;
    return cmd;
}



function start_click(data){
    s_ui.datagrid_clear("grid1");
    s_ui.datagrid_init_column("grid1",5,"字段,good,bad,WOE,IV");
    
    var data=s_file.Ini_Read(file_ini,"next","value");
    process_step11(data);
    
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
                
            s_ui.Web_Content("web",css_head+log_msg);
        }
    }
}

//发送消息 
function send_msg_click(){
    msg_id+=1;
    
    var strMsg=s_ui.text_read("txt_send");
    send_msg("cmd",friend,strMsg,"step:"+step);
    
    
    s_ui.text_set("txt_send","");
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
        s_ui.Web_Content("web",css_head+log_msg);
    }else{
        s_ui.status_label_show("status_label","token==null!");
    }
}


s_ui.splitcontainer_init("split",0,0,500,500,"h");
s_ui.splitcontainer_distance("split",30);


s_ui.text_init("txt_file",s_sys.value_read("file"),350,450,200,30);


//界面
s_ui.datagrid_init("grid1",10,60,650,200);

s_ui.text_init("txt_send","ls",380,350,320,30);


s_ui.button_init("b1_send","发送",600,400,100,30,"send_msg_click","");



s_ui.textbox_init("txt_user_name","000",10,450,200,30);


s_ui.textbox_init("txt_info","",10,250,300,50);


s_ui.panel_init("panel_top1",0,0,500,25,"none");

s_ui.splitcontainer_add("split",0,"panel_top1","fill");

s_ui.panel_add("panel_top1","txt_info","fill");
s_ui.panel_add("panel_top1","txt_user_name","right");


s_ui.Web_Init("web",250,60,450,250);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.progress_init("progress2",100,400,500,30);

s_ui.splitcontainer_add("split",1,"web","fill");

s_ui.splitcontainer_add("split",1,"progress2","top");

s_ui.splitcontainer_add("split",1,"grid1","top");
s_ui.splitcontainer_add("split",1,"txt_file","top");



s_ui.panel_init("panel2",0,0,500,50,"none");
s_ui.splitcontainer_add("split",1,"panel2","bottom");


s_ui.button_init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat2\\step5");
s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat2\\step6");


s_ui.panel_add("panel2","b_pre","right");
s_ui.panel_add("panel2","b_next","right");


s_ui.status_init("status",0,0,200,30,"bottom");
s_ui.status_label_init("status_label","",100,30);
s_ui.status_add("status","status_label","left");
s_ui.status_label_init("status_label2","",100,30);
s_ui.status_add("status","status_label2","left");



s_ui.button_default("b1_send");
s_ui.show_form(800,600);
s_ui.Form_Title("v2 WOE IV计算");


s_sys.tcp_event();

on_load("");

data_init("");

start_click("");