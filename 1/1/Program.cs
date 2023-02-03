using IBApi;
using Samples;

//set up connection
EWrapperImpl testImpl = new EWrapperImpl();

EClientSocket clientSocket = testImpl.ClientSocket;
EReaderSignal readerSignal = testImpl.Signal;

clientSocket.eConnect("127.0.0.1", 7497, 0);
var reader = new EReader(clientSocket, readerSignal);
reader.Start();

new Thread(() => { while (clientSocket.IsConnected()) { readerSignal.waitForSignal(); reader.processMsgs(); } }) { IsBackground = true }.Start();

while (testImpl.NextOrderId <= 0) { }

//place order

Order GentasOrder = OrderSamples.MarketOrder("Buy", 15);

clientSocket.placeOrder(200, ContractSamples.USStock(), GentasOrder);

Console.WriteLine("Disconnecting...");
clientSocket.eDisconnect();
return 0;