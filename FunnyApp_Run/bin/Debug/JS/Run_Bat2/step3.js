
[[[..\\data\\default.js]]]
[[[..\\data\\common_string.js]]]
[[[..\\data\\tcp.js]]]
[[[..\\data\\run_bat_common.js]]]

var file_memo=disk+"\\Net\\Web\\Data\\memo.ini";
var file_ini=disk+"\\Net\\Web\\main.ini";
var friend=s_file.Ini_Read(file_ini,"main","friend_selected");

var friend_return=0;
var msg_id=0;
var sep=1;
var step=0;//处理步骤
var row_index=0;//第几个字段被点击

var log_msg="";
var keep_count=1;

var myMap=[];
var head="";



function event_msg(data){
    var obj=JSON.parse(data);
    var msg=obj.message;
    //s_ui.msg(data);
    
    switch(obj.type){
        case "chat_return":
            log_msg="<b>chat_return:"+obj.oid+"</b><br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
            s_ui.status_label_show("status_label2",msg);
            delete myMap["K"+obj.oid];
            break;
        case "login.ok":
            friend_list("");
            break;
        case "list.all":
            s_ui.listbox_add("list_friend",obj.message);
            friend_return=1;
            break;
        case "file_sql":
            if (obj.message=="finished"){
                
                log_msg="<b>finished "+obj.from+";"+step+"</b><br>"+log_msg;
                s_ui.Web_Content("web",css_head+log_msg);
                switch (obj.from){
                case "/root/step3.txt":
                    if (step==1){
                        process_step(obj);
                    }else{
                        s_ui.Web_Content("web","多次接收："+step+"="+data);
                    }
                    break;
                case "/root/step3_2.txt":
                    if (step==2){
                        log_msg="<b>process_step "+obj.from+";"+step+"</b><br>"+log_msg;
                        s_ui.Web_Content("web",css_head+log_msg);
                        s_ui.status_label_show("status_label","/root/step3_v2.txt step2 process_step");
                        process_step(obj);
                    }else{
                        s_ui.Web_Content("web","多次接收："+step+"="+data);
                    }
                    break;
                }
            }
            break;
        case "msg":
            switch(obj.return_cmd){
                case "step:3":
                case "step:11":
                case "step:12":
                    process_step(obj);
                    break;
                default:
                    switch(obj.from){
                        case "line":
                            s_ui.datagrid_clear("grid1");
                            s_ui.datagrid_init_column("grid1",3,"字段,类型,C");
                            var strSplit=msg.split(",");
                            for(var i=0;i<strSplit.length;i++){
                                if (strSplit[i].startsWith("0.0")){
                                    s_ui.datagrid_add_line("grid1",(i+1)+",数字,"+strSplit[i],",");
                                }else{
                                    s_ui.datagrid_add_line("grid1",(i+1)+",字符,"+strSplit[i],",");
                                }
                            }
                            s_ui.datagrid_add_button("grid1","modify","映射","map_click");
                    }
                    break;
            }
            log_msg=s_time.Time_Now()
        +" <span style='color:blue;'>"+obj.from+"</span>"
        +obj.return_cmd+"<br>"+"<pre>"+msg+"</pre>"
        +"<br>"+log_msg;
            s_file.append(disk+"\\Net\\Web\\log\\"+obj.from+".txt",
                s_time.Date_Now()+" "+s_time.Time_Now()+" "+msg+"\r\n");
            
            s_ui.Web_Content("web",css_head+log_msg);
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
            log_msg=s_time.Time_Now()
                +"<span style='color:red;'>"+obj.from+"</span><br>"
                +msg+"<br><br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
            break;
    }
}

function process_step(obj){
    var msg=obj.message;
    
    var line2="";
    if (step<3){
        s_ui.status_label_show("status_label","step1");
        var file1=s_file.Ini_Read(file_memo,"main","file1");
        if (file1=="") file1="E:\\sample1.txt";
        s_ui.status_label_show("status_label","step2");
        var line=s_file.read(file1,1);
        var strSplit=line.split(",");
        var count=strSplit.length;
        
        for (var i=1;i<=count;i++){
            line2+="count(1)-sum(c"+i+"),";
        }
        if (line2.endsWith(",")){
            line2=line2.substr(0,line2.length-1);
        }
    }
    
    var cmd="";
    switch(step){
        case 1:
            cmd="file_sql /root/happyli/set_hadoop.ini "+userName+" /root/step3.txt 250000 \"select "+line2+" from t;\" , /root/step3_2.txt";
            step=2;
            send_msg("cmd",friend,cmd,"step:2");
            break;
        case 2:
            s_ui.status_label_show("status_label","step=2");
            do_step3("");
            break;
        case 3:
            s_ui.datagrid_clear("grid1");
            s_ui.datagrid_init_column("grid1",3,"字段,类型,C");
            var strSplit=msg.split(",");
            for(var i=0;i<strSplit.length;i++){
                if (strSplit[i].startsWith("0.0")){
                    s_ui.datagrid_add_line("grid1",(i+1)+",数字,"+strSplit[i],",");
                }else{
                    s_ui.datagrid_add_line("grid1",(i+1)+",字符,"+strSplit[i],",");
                }
            }
            s_ui.datagrid_add_button("grid1","modify","映射","map_click");
            break;
        case 11:
            cmd="cat /root/map_"+row_index+".txt";
            step=12;
            send_msg("cmd",friend,cmd,"step:12");
            break;
        case 12:
            msg=msg.replaceAll("\\\\n","\n");
            s_sys.value_save("map",msg);
            s_ui.Run_JS("Run_Bat2\\step3_map.js");
            break;
    }
}

function do_step3(data){
    cmd="cat /root/step3_2.txt";
    step=3
    send_msg("cmd",friend,cmd,"step:3");
}

function sql(file1,sql,sep,output){
    var cmd="file_sql /root/happyli/set_hadoop.ini "+userName+" "+file1
        +" 250000 \""+sql+"\" "+sep+" "+output;
    return cmd;
}


function map_click(data){
    
    var index=parseInt(data);
    row_index=s_ui.datagrid_read("grid1",index,0);
    s_sys.value_save("row_index",row_index);
    
    
    var file=disk+"\\Net\\Web\\data\\map_c"+row_index+".txt";
    if (s_file.exists(file)){
        var msg=s_file.read(file);
        s_sys.value_save("map",msg);
        s_ui.Run_JS("Run_Bat2\\step3_map.js");
        return ;
    }
    
    var type=s_ui.datagrid_read("grid1",index,1);
    //s_ui.msg(type);
    
    var file2=s_file.Ini_Read(file_memo,"main","file2");
    if (file2=="") file2="/upload/sample1.txt";
    
    var cmd=sql("/home/ftp_home"+file2,
            "select c"+row_index+",count(1) from t group by c"+row_index,
            ",","/root/map_"+row_index+".txt");
    step=11;
    send_msg("cmd",friend,cmd,"step:11");
    
    
}

function clear_data(data){
    s_ui.datagrid_clear("grid1");
}



function data_init(data){
    s_ui.datagrid_clear("grid1");
    
    s_ui.datagrid_init_column("grid1",3,"字段,类型,C");
    s_ui.datagrid_add_line("grid1","1,点击统计按钮！",",");
    
    s_ui.datagrid_add_button("grid1","modify","映射","map_click");
}

function static_click(data){
    var file1=s_file.Ini_Read(file_memo,"main","file1");
    if (file1=="") file1="E:\\sample1.txt";
    var line=s_file.read(file1,1);
    var strSplit=line.split(",");
    var count=strSplit.length;
    
    var line="";
    for (var i=1;i<=count;i++){
        line+="isnumeric(c"+i+"),";
    }
    if (line.endsWith(",")){
        line=line.substr(0,line.length-1);
    }
    
    var file2=s_file.Ini_Read(file_memo,"main","file2");
    if (file2=="") file2="/upload/sample1.txt";
    
    //var cmd=sql("/home/ftp_home"+file2,"select "+line+" from t;",",","/root/step3.txt");
        
    var cmd="run_js2 /root/happyli/app/field_type.js 0 "+userName+" /home/ftp_home"+file2;
    
    step=1;
    send_msg("cmd",friend,cmd,"step:1");
    
}


function next_click(data){
    
    s_ui.Run_JS(data+".js");
    s_ui.close();
}


function on_load(){
    userName=sys_read_ini()+"/linux_bat2";
    s_ui.status_label_show("status_label",userName);
        
    //s_ui.text_set("txt_user_name",userName);
}

function friend_list(data){
    
    s_ui.listbox_clear("list_friend");
    s_ui.listbox_add("list_friend","*");

    send_msg("friend_list","","","friend_list");
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
        
        log_msg=s_time.Time_Now()+" 我 &gt; <span style='color:gray;'>"+friend+"</span><br>"
                +msg+"<br><br>"
                +"<b>tcp.send:"+strLine+"</b>"
                +log_msg;
        s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",
            s_time.Date_Now()+" "+s_time.Time_Now()+" "+msg+"\r\n");
        
        s_ui.Web_Content("web",css_head+log_msg);
    
    
        s_tcp.send("m:<s>:"+strLine+":</s>");
        
        
    }else{
        s_ui.status_label_show("status_label","token==null!");
    }
    
}




function friend_change(data){
    
    var friend=s_ui.listbox_text("list_friend");
    if (friend!=""){
        //s_sys.value_save("friend_selected",friend);
        s_file.Ini_Save(disk+"\\Net\\Web\\main.ini","main","friend_selected",friend);
    }
}

function resend_chat_msg(data) {
    for(var key in myMap){
        var pMsg=myMap[key];
        pMsg.Count+=1;
        if (pMsg.Count<10){
            ;
        }else if (pMsg.Count==5){//再发送一次
            log_msg+="<b>resend:"+pMsg.Msg+"</b>";
            s_ui.Web_Content("web",css_head+log_msg);
            s_tcp.send("m:<s>:"+pMsg.Msg+":</s>");
        }else{
            var obj=JSON.parse(pMsg.Msg);
            var friend=pMsg.to;
            log_msg=s_time.Time_Now()+" <font color=red>(消息发送失败) </font> <span style='color:gray;'>"
                    +obj.id+"="+obj.to+"</span><br>"
                    +obj.message+"<br><br>"+log_msg;
            s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",
                s_time.Date_Now()+" "+s_time.Time_Now()+" 消息发送失败："+obj.id+"="+obj.message+"\r\n");
            delete myMap["K"+obj.id];
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



//界面
s_ui.datagrid_init("grid1",10,60,650,120);


s_ui.Web_Init("web",250,60,450,250);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");

s_ui.progress_init("progress2",100,400,500,30);

s_ui.panel_init("panel_content",0,0,500,200,"fill");

s_ui.panel_init("panel1",0,0,500,300,"top");

s_ui.panel_add("panel1","grid1","fill");
s_ui.panel_add("panel1","progress1","top");
s_ui.panel_add("panel1","progress2","top");

s_ui.panel_init("panel2",0,0,500,50,"bottom");


s_ui.panel_add("panel_content","web","fill");



s_ui.button_init("b_static","统计",400,350,200,30,"static_click","");
s_ui.button_init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat2\\step2");
s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat2\\step4");


s_ui.panel_add("panel2","b_static","right");
s_ui.panel_add("panel2","b_pre","right");
s_ui.panel_add("panel2","b_next","right");



s_ui.menu_init("Menu1");
s_ui.menu_add("Menu1","Menu_File","&File");

s_ui.menu_add("Menu1","Tools","&Tools");
s_ui.menu_item_add("Menu1","Tools","Menu_Static","重新统计分析","static_click","");
s_ui.menu_item_add("Menu1","Tools","Menu_Static","最后一步","do_step3","");


//状态栏 开始
s_ui.status_init("status",0,0,200,30,"bottom");
s_ui.status_label_init("status_label","111",100,30);
s_ui.status_add("status","status_label","left");
s_ui.status_label_init("status_label2","222",100,30);
s_ui.status_add("status","status_label2","left");
//状态栏 结束


s_ui.show_form(800,600);
s_ui.Form_Title("v2 第3步 字段映射");

s_sys.tcp_event();

on_load("");

data_init("");



