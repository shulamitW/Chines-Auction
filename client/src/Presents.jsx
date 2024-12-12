import React, { useState, useEffect } from 'react';
import { Button } from 'primereact/button';
import { DataView, DataViewLayoutOptions } from 'primereact/dataview';
import { Sidebar } from 'primereact/sidebar';
import { classNames } from 'primereact/utils';
import  MenuBarUser from './menuBarUser'
import {Dialog} from 'primereact/dialog'
import { Column } from 'primereact/column';
import axios from 'axios';
axios.defaults.baseURL="http://localhost:5018"

export default function Presents() {
    const [presents, setPresents] = useState([]);
    const [cart, setCart] = useState([]);
    const [products, setProducts] = useState([]);
    const [layout, setLayout] = useState('grid');
    const [visible, setVisible] = useState(false);
    const [visibleCheckOut, setVisibleCheckOut] = useState(false);
    const [userId, setUserId] = useState(0);
    //const userId = 1
    //const [role, setRole] = useState("");
    const readUserIdFromLocalStorage = () => {
        const id = localStorage.getItem("loggedInUser");
        console.log(id ,"iddddddddddddd");
        setUserId(Number(id)); // Convert to number
    };

const getAllPresents= async()=>
{
    try{
       let tmp= await axios.get(`/Present/GetPresents`).then(res=>
        {
            console.log(res.data);
            return res.data
        }) 
       console.log(tmp)
       setPresents(tmp)     
    }   
    catch(err){
        alert(err)
    }
}
    useEffect(() => {
        // presentservice.getpresents().then((data) => setpresents(data.slice(0, 12)));
        getAllPresents();
        readUserIdFromLocalStorage();
    }, []);
    
    useEffect(() => {
        if (userId !== 0) {
            getShoppingCart(userId);
        }
    }, [userId]);

    const listItem = (present, index) => {
        return (
            <div className="col-12" key={present.id}>
                <div className={classNames('flex flex-column xl:flex-row xl:align-items-start p-4 gap-4', { 'border-top-1 surface-border': index !== 0 })}>
                    <img className="w-9 sm:w-16rem xl:w-10rem shadow-2 block xl:block mx-auto border-round" src={present.imagePath} alt={present.name} />
                    <div className="flex flex-column sm:flex-row justify-content-between align-items-center xl:align-items-start flex-1 gap-4">
                        <div className="flex flex-column align-items-center sm:align-items-start gap-3">
                            <div className="text-2xl font-bold text-900">{present.name}</div>
                            {console.log(present)}
                            {/* <Rating value={present.rating} readOnly cancel={false}></Rating> */}
                            <div className="flex align-items-center gap-3">
                                <span className="flex align-items-center gap-2">
                                    <i className="pi pi-tag"></i>
                                    <span className="font-semibold">{present.category.description}</span>
                                </span>
                                {/* <Tag value={present.inventoryStatus} severity={getSeverity(present)}></Tag> */}
                            </div>
                        </div>
                        <div className="flex sm:flex-column align-items-center sm:align-items-end gap-3 sm:gap-2">
                            <span className="text-2xl font-semibold">${present.price}</span>
                            {present.winnerId==null?
                             <Button icon="pi pi-shopping-cart" className="p-button-rounded" onClick={()=>addToCart(present)}></Button>
                             :<span className="font-semibold">The lucky winner is: {present.winner.fullName}</span>}
                        </div>
                    </div>
                </div>
            </div>
        );
    };
    const gridItem = (present) => {
            return (
                <div className="col-12 sm:col-6 lg:col-12 xl:col-4 p-2" key={present.Id}>
                    <div className="p-4 border-1 surface-border surface-card border-round">
                        <div className="flex flex-wrap align-items-center justify-content-between gap-2">
                            <div className="flex align-items-center gap-2">
                                <i className="pi pi-tag"></i>
                                {console.log(present)}
                                <span className="font-semibold">{present.category.description}</span>
                            </div>
                            {/* <Tag value={present.inventoryStatus} severity={getSeverity(present)}></Tag> */}
                        </div>
                        <div className="flex flex-column align-items-center gap-3 py-5">
                            <img className="w-9 shadow-2 border-round" src={present.imagePath} alt={present.name} />
                            <div className="text-2xl font-bold">{present.name}</div>
                            {/* <Rating value={present.rating} readOnly cancel={false}></Rating> */}
                        </div>
                        <div className="flex align-items-center justify-content-between">
                            <span className="text-2xl font-semibold">${present.price}</span>
                            {present.winnerId==null?
                             <Button icon="pi pi-shopping-cart" className="p-button-rounded" onClick={()=>addToCart(present)}></Button>
                             :<span className="font-semibold">The lucky winner is: {present.winner.fullName}</span>}
                        </div>
                    </div>
                </div>
            );
        };
        
    // const  addToCart = async (present) =>{
    //      try {
    //         console.log(
    //           "---------------------------------------------- add present --- ",
    //           present
    //         );
    //         await axios
    //         //http://localhost:5018/Purchase/AddToCart?presentId=9&userId=1
    //           .post(`/Purchase/AddToCart/${present.id}`)
    //           .then((res) => console.log(res.data.name));
    //         getAllPresents();
    //       } catch (err) {
    //         alert(err);
    //       }
    // }

    const addToCart = async (present) => {
        try {
            const tmpId = present.presentId != null ? present.presentId: present.id
            const params = new URLSearchParams({
            cartId: cart.id, // Your cartId value
            presentId: tmpId, // Your presentId value
            userId: userId, // Your userId value
          });
      
          const url = `/Purchase/AddToCart?${params}`;
      
          console.log("Sending request to:", url);
      
          await axios.post(url)
            .then((res) => console.log(res.data.name));
           // debugger
            getShoppingCart(userId);
            getAllPresents();
        } catch (err) {
        alert(err);
        }
      };

      
      const checkOut = async () => {
        try {
          console.log("cart", cart);
      
          // First await should be for axios.post
          await axios.post(`/Purchase/Buy?cartId=${cart.id}`)
            .then((res) => {
              console.log(res.data.name);
            });
      
          setVisibleCheckOut(true);
      
          // Perform subsequent async calls after the initial await
           getAllPresents();
          //debugger
          ////////dont workkkkkk:(
           //getShoppingCart(userId);
      
        } catch (err) {
          console.error("Error during checkout:", err);
          alert(err);
        }
      };
      
      const RemoveFromCart = async (present) => {
        try {
          const params = new URLSearchParams({
            cartId: cart.id, // Your cartId value
            presentId: present.presentId , // Your presentId value
            userId: userId, // Your userId value
          });
      
          const url = `/Purchase/RemoveFromCart?${params}`;
      
          console.log("Sending request to:", url);
      
          await axios.post(url)
            .then((res) => console.log(res.data.name));
            getShoppingCart(userId);
        } catch (err) {
          alert(err);
        }
      };
 
    const getShoppingCart = async (userId) => {
        //debugger
        try {
          let tmp = await axios.post(`/Purchase/GetShoppingCartById/${userId}`)
            .then(res => {
              console.log(res.data);
              return res.data;
            });
            setCart(tmp);
            } catch (err) {
          console.error("Error fetching shopping cart:", err);
          throw err; // Rethrow the error to be caught in checkOut
        }
      };
    const itemTemplate = (product, layout, index) => {
        if (!product) {
            return;
        }
            if (layout === 'list') return listItem(product, index);
            else if (layout === 'grid') return gridItem(product);
        
      
        
    };

    const CartCount = () => {
        if (cart && cart.tickets) {
            return cart.tickets.length;
        } else {
            return 0; // Or any default value based on your requirements
        }
    }
    const listTemplate = (presents, layout) => {
        return <div className="grid grid-nogutter">{presents.map((present, index) => itemTemplate(present, layout, index))}</div>;
    };

    const header = () => {
        return (
            <>
            <div className="flex justify-content-end">
                <DataViewLayoutOptions layout={layout} onChange={(e) => setLayout(e.value)} />
            </div>

            <div className="card flex justify-content-left">
                <Sidebar visible={visible} onHide={() => setVisible(false)} className="w-full md:w-20rem lg:w-30rem">
                <h2>Shopping Cart</h2>  
                <div className="card">
                <DataView value={cart.tickets} listTemplate={listTemplateCart} paginator rows={5} />
                <Button onClick={() => checkOut()}>check out</Button>

                </div>
                </Sidebar>
                {/* <Button icon="pi pi-shopping-cart" onClick={() => setVisible(true)} /> */}
            </div>           
        </>
        );
    };

    const itemTemplateCart = (product, index) => {
        return (
            <div className="col-12" key={product.id}>
                <div className={classNames('flex flex-column xl:flex-row xl:align-items-start p-4 gap-4', { 'border-top-1 surface-border': index !== 0 })}>
                    <img className="w-9 sm:w-16rem xl:w-10rem shadow-2 block xl:block mx-auto border-round" src={product.present.imagePath} alt={product.present.name} />
                    <div className="flex flex-column sm:flex-row justify-content-between align-items-center xl:align-items-start flex-1 gap-4">
                        <div className="flex flex-column align-items-center sm:align-items-start gap-3">
                            <div className="text-2xl font-bold text-900">{product.present.name}</div>
                            {/* <Rating value={product.rating} readOnly cancel={false}></Rating> */}
                            <div className="flex align-items-center gap-3">
                                <span className="flex align-items-center gap-2">
                                    <i className="pi pi-tag"></i>
                                    <span className="font-semibold">{product.present.category.description}</span>
                                </span>
                            </div>
                        </div>
                        <div className="flex sm:flex-column align-items-center sm:align-items-end gap-3 sm:gap-2">
                            <span className="text-2xl font-semibold">${product.present.price}</span>
                            {/* <Button icon="pi pi-trash" className="p-button-rounded"></Button> */}
                            <div className="flex">
                            <Button icon="pi pi-minus" rounded text onClick={()=>RemoveFromCart(product)}/>
                            <span className="text-1.5xl font-semibold">{product.quantity}</span>
                            <Button icon="pi pi-plus" rounded text  onClick={()=>addToCart(product)}/>

                            </div>
                          

                        </div>
                    </div>
                </div>
            </div>
        );
    };

    const listTemplateCart = (items) => {
        if (!items || items.length === 0) return null;

        let list = items.map((product, index) => {
            return itemTemplateCart(product, index);
        });

        return <div className="grid grid-nogutter">{list}</div>;
    };

    return (
        <>
        <MenuBarUser openCart={() => setVisible(true)} cartCount={CartCount()}></MenuBarUser>
        <div className="card">
            <DataView value={presents} listTemplate={listTemplate} layout={layout} header={header()} />
        </div>
        <Dialog header="Thank you :)" visible={visibleCheckOut} modal={false} style={{ width: '50vw' }} onHide={() => {if (!visibleCheckOut) return; setVisibleCheckOut(false); }}>
                <p className="m-0">
                    Thank you for your donation! 
                    <br></br>
                    A message will be send to the winners.
                </p>
            </Dialog>
        </>
    )
}