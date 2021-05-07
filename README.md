# VirtualConnectorAvalonia
Application for connecting streams

This application is designed to connect various communication channels with each other.
When data is received by any of the communication channels, it is forwarded to all other communication channels combined in one group.
For example, by connecting the COM port to the TCP server, it becomes possible to forward the COM port remotely.
And if you connect a TCP client and a TCP server, you can listen for data on the socket without using WireShark. 

Это приложение предназначено для соединения различных каналов связи между собой. 
Когда данные принимаются любым из каналов связи, они перебрасываются во все остальные каналы связи, объединенные в одной группе.
К примеру соеденив COM порт с TCP сервером становится возможным пробросить удаленно COM порт.
А если соеденить TCP клиент и TCP сервер, можно прослушивать данные на сокете без использования WireShark.
