RESTORE DATABASE Олег
FROM DISK = 'C:\DataBases\Олег.bak'
WITH NORECOVERY,
MOVE 'Олег' TO 'C:\DataBases\Олег.mdf',
MOVE 'Олег_log' TO 'C:\DataBases\Олег.ldf';
RESTORE LOG Олег
FROM DISK = 'C:\DataBases\Олег.bak'
WITH RECOVERY;