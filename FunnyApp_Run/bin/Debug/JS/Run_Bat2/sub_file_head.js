[[[..\data\default.js]]]
[[[..\data\common_string.js]]]
[[[..\data\tcp.js]]]
[[[..\data\run_bat_common.js]]]


var file_memo=disk+"\\Net\\Web\\Data\\memo.ini";
var file_ini=disk+"\\Net\\Web\\main.ini";
var friend=s_file.Ini_Read(file_ini,"main","friend_selected");



var file1=s_sys.value_read("file1");
var file2=s_sys.value_read("file2");

s_file.read_begin("file1",file1);
s_file.write_begin("file2",file2,false);

var line=s_file.read_line("file1");
var strSplit=line.split(",");
for (var i=0;i<strSplit.length;i++){
    var k=i+1;
    s_file.Ini_Save(file_memo,"main","memo"+k,strSplit[i]);
}

line=s_file.read_line("file1");
while(line!=null){
    s_file.write_line("file2",line);
    line=s_file.read_line("file1");
}
s_file.read_end("file1");
s_file.write_end("file2");