<?php
Include_once ('mybase.class.php');
$mail=trim($_GET['mail']);
$MyBaseReg = new MyBase();
$SID =trim($_GET['SID']);
$textfield = $SID;
$textfield = str_replace('`','',$textfield);
$textfield = str_replace('%','',$textfield);
$textfield = str_replace(',','',$textfield);
$textfield = str_replace('"','',$textfield);
$textfield = str_replace(';','',$textfield);
$textfield = str_replace(':','',$textfield);
$textfield = str_replace("'",'',$textfield);
$textfield = htmlspecialchars($textfield);
$SID=$textfield;
$MyBaseReg->actionUser($SID,base64_encode($mail));
unset ($MyBaseReg);
header('Location: index.php');
?>