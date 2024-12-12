import React, { useState, useEffect, useRef } from "react";
// import { presentservice } from './service/presentservice'; // presents
import { Button } from "primereact/button";
import { DataView, DataViewLayoutOptions } from "primereact/dataview";
// import { Rating } from 'primereact/rating';
import { Tag } from "primereact/tag";
import { classNames } from "primereact/utils";
import axios from "axios";
import { Toast } from "primereact/toast";
import { InputText } from "primereact/inputtext";
import { Dialog } from "primereact/dialog";
import { InputTextarea } from "primereact/inputtextarea";
import { InputNumber } from "primereact/inputnumber";
import { CIcon } from "@coreui/icons-react";
import { Dropdown } from "primereact/dropdown";
import {Avatar} from "primereact/avatar"
import Home from "./menuBarManager";
axios.defaults.baseURL = "http://localhost:5018";

export default function PresentsManagement() {
  let emptyPresent = {
    id: null,
    categoryId: null,
    name: "",
    price: 0.0,
    donorId: null,
    imagePath: "",
    description: "",
  };

  const [presents, setPresents] = useState([]);
  const [layout, setLayout] = useState("list");
  const [deleteProductDialog, setDeleteProductDialog] = useState(false);
  const [productDialog, setProductDialog] = useState(false);
  const [submitted, setSubmitted] = useState(false);
  const [present, setPresent] = useState(emptyPresent);
  const [sender, setSender] = useState("new");
  const toast = useRef(null);
  const [value, setValue] = useState(present.price, present);
  const [selectedDonor, setSelectedDonor] = useState(null);
  const [DonorsList, setDonorsList] = useState([]);
  const [winner, setWinner] = useState(null);

  const [selectedCategory, setselectedCategory] = useState(null);
  const [CategoryList, setCategoryList] = useState([]);
  const [presentDonorList, setPresentDonorList] = useState([]);

  const addPresent = async (present) => {
    try {
      console.log(
        "---------------------------------------------- add present --- ",
        present
      );
      await axios
        .post(`/Present/AddPresents`, present)
        .then((res) => console.log(res.data.name));
      getAllPresents();
    } catch (err) {
      alert(err);
    }
  };

  const getAllPresents = async () => {
    try {
      let tmp = await axios.get(`/Present/GetPresents`).then((res) => {
        console.log(res.data);
        return res.data;
      });
      console.log("getAllPresents--------------------", tmp);
      setPresents(tmp);
      await GetPresentWithDonor(tmp)
    } catch (err) {
      // alert(err);
      console.log(err);
    }
  };

  const getAllCategories = async () => {
    try {
      let tmp = await axios.get(`/Category/GetCategory`).then((res) => {
        console.log(res.data);
        return res.data;
      });
      console.log(tmp);
      setCategoryList(tmp);
    } catch (err) {
      alert(err);
    }
  };

  const getAllDonors = async () => {
    try {
      let tmp = await axios.get(`/Donor/GetDonors`).then((res) => {
        console.log(res.data);
        return res.data;
      });

      console.log("---------------------------", tmp);
      setDonorsList(tmp);
    } catch (err) {
      alert(err);
    }
  };

  const getDonorById = async (id) => {
    console.log("getDonorById/////////////////", id);
    try {
      let tmp = await axios.get(`/Donor/GetDonorById?donorId=${id}`).then((res) => {
        console.log(res.data);
        return res.data;
      });

      console.log("---------------------------", tmp);
      setSelectedDonor(tmp);
      return tmp;
    } catch (err) {
      alert(err);
    }
  };

  const GetPresentWithDonor = async (tmp) => {
    let PresentDonorTmp = [];

    for (let i = 0; i < tmp.length; i++) {
      const updatedObject = await aa(tmp[i]);
      PresentDonorTmp.push(updatedObject);
    }
    setPresentDonorList(PresentDonorTmp);
  };

  const aa = async (tmp) => {
    const DonorObj = await getDonorById(tmp.donorId);
    const updatedObject = { ...tmp, Donor: DonorObj };

    return updatedObject;
  };

  useEffect(() => {
    // presentservice.getpresents().then((data) => setpresents(data.slice(0, 12)));
    getAllPresents();
    getAllDonors();
    getAllCategories();
  }, []);

  const listItem = (present, index) => {
    return (
      <div className="col-12" key={present.id}>
        <div
          className={classNames(
            "flex flex-column xl:flex-row xl:align-items-start p-4 gap-4",
            { "border-top-1 surface-border": index !== 0 }
          )}
        >
          <img
            className="w-9 sm:w-16rem xl:w-10rem shadow-2 block xl:block mx-auto border-round"
            src={present.imagePath}
            alt={present.imagePath}
          />
          <div className="flex flex-column sm:flex-row justify-content-between align-items-center xl:align-items-start flex-1 gap-4">
            <div className="flex flex-column align-items-center sm:align-items-start gap-3">
              <div className="text-2xl font-bold text-900">{present.name}</div>
              <div className="flex align-items-center gap-3">
                <span className="flex align-items-center gap-2">
                  <i className="pi pi-tag"></i>
                  <span className="font-semibold">
                    {present.category.description}
                  </span>
                </span>
              
              </div>
              <div className="flex align-items-center gap-2">
                  <span className="text-1.5xl font-bold text-900">Donor</span>
                  <Avatar image="https://primefaces.org/cdn/primereact/images/avatar/amyelsner.png" shape="circle" />
                  <span className="font-semibold">{present.Donor.fullName}</span>
                </div>
            </div>
            <div className="flex sm:flex-column align-items-center sm:align-items-end gap-3 sm:gap-2">
              <span className="text-2xl font-semibold">${present.price}</span>
              <Button
                icon="pi pi-pencil"
                rounded
                outlined
                className="mr-2"
                onClick={() => openEditDialog(present.id, present)}
              />
              <Button
                icon="pi pi-trash"
                rounded
                outlined
                severity="danger"
                onClick={() => deletePresent(present.id)}
              />
              {
              present.winnerId ==null? 
               <Button
                icon="pi pi-trophy"
                rounded
                outlined
                className="mr-2"
                onClick={() => raffle(present.id)}/> :<span >The  lucky winner is: {present.winner.fullName}</span>}
              
             {/* <button icon={cidDonate} onClick={() => deletePresent(present.id)}/> */}
            </div>
          </div>
        </div>
      </div>
    );
  };
  useEffect(() => {

  }, [])
  const openEditDialog = async (presentId, present) => {
    {
      console.log("present in edit------------------", present);
    }

    await getDonorById(present.donorId);
    setselectedCategory(present.category)
    // setSelectedDonor(present.donor.name)
    setPresent({ ...present });
    setProductDialog(true);
    setSender("edit");
  };
  const deletePresent = async(presentId) => {

    try {
      setDeleteProductDialog(false);

      console.log("--- deleted present --- ", presentId);

      await axios.delete(`/Present/RemovePresent/${presentId}`).then((res) => {
         getAllPresents();
      });
    } catch (err) {
      alert(err);
    }
  };

  const raffle = async (presentId) => {
    try {
      debugger
      setWinner(null);
      const response = await axios.get(`/Raffle/PresentRaffle`, {
        params: {
          presentId: presentId
        }
      });
      //debugger

      console.log(response.data);
      setWinner(response.data.winnerId);
      await getAllPresents();
      return response.data;
    } catch (err) {
      console.log(err);
    }
  };
  
  const hideDialog = () => {
    setSubmitted(false);
    setProductDialog(false);
  };

  const onInputChange = (e, name) => {
    const val = (e.target && e.target.value) || "";
    let _product = { ...present };

    _product[`${name}`] = val;
    setPresent(_product);
  };

  const saveProduct = async () => {
    setSubmitted(true);
    const tmpPresent = {
      categoryId: selectedCategory.id,
      name: present.name,
      price: present.price,
      donorId: selectedDonor.id,
      imagePath: present.imagePath,
      description: present.description,
    };
    {
      console.log("*********", present);
    }
    if (
      present.name.trim() &&
      present.imagePath.trim() &&
      present.description.trim() &&
      selectedCategory.id &&
      selectedDonor.id &&
      present.price
    ) {
      if (sender == "edit") {
        {
          console.log("tmp present*****", tmpPresent);
        }

        await editPresent(present.id, tmpPresent);
        toast.current.show({
          severity: "success",
          summary: "Successful",
          detail: "Product Updated",
          life: 3000,
        });
      } else {
        //if sender=='new'
        addPresent(tmpPresent);
      }
    } //if there is null
    else
      toast.current.show({
        severity: "error",
        summary: "Error Message",
        detail: "Validation failed",
      });
    // setProducts(_products);
    setProductDialog(false);
    setPresent(emptyPresent);
  };
  const editPresent = async (presentId, present) => {
    try {
      await axios
        .put(`/Present/UpdatePresent/${presentId}`, present)
        .then(async (res) => {
          console.log("--- edit present --- ", presentId);
          await getAllPresents();
        });
    } catch (err) {
      alert(err);
    }
  };
  const productDialogFooter = (
    <React.Fragment>
      <Button label="Cancel" icon="pi pi-times" outlined onClick={hideDialog} />
      <Button label="Save" icon="pi pi-check" onClick={saveProduct} />
    </React.Fragment>
  );
  const gridItem = (present) => {
    return (
      <div className="col-12 sm:col-6 lg:col-12 xl:col-4 p-2" key={present.Id}>
        <div className="p-4 border-1 surface-border surface-card border-round">
          <div className="flex flex-wrap align-items-center justify-content-between gap-2">
            <div className="flex align-items-center gap-2">
              <i className="pi pi-tag"></i>
              {console.log(present)}
              <span className="font-semibold">
                {present.category.description}
              </span>
            </div>
            {/* <Tag value={present.inventoryStatus} severity={getSeverity(present)}></Tag> */}
          </div>
          <div className="flex flex-column align-items-center gap-3 py-5">
            <img
              className="w-9 shadow-2 border-round"
              src={present.imagePath}
              alt={present.name}
            />
            <div className="text-2xl font-bold">{present.name}</div>
            {/* <Rating value={present.rating} readOnly cancel={false}></Rating> */}
          </div>
          <div className="flex align-items-center gap-2">
            <span className="text-1.5xl font-bold text-900">Donor</span>
            <Avatar image="https://primefaces.org/cdn/primereact/images/avatar/amyelsner.png" shape="circle" />
            <span className="font-semibold">{present.Donor.fullName}</span>
          </div>
          <div className="flex align-items-center justify-content-between">
            <span className="text-2xl font-semibold">${present.price}</span>
            <Button
              icon="pi pi-pencil"
              rounded
              outlined
              className="mr-2"
              onClick={() => openEditDialog(present.id, present)}
            />
            <Button
              icon="pi pi-trash"
              rounded
              outlined
              severity="danger"
              onClick={() => deletePresent(present.id)}
            />
          </div>
        </div>
      </div>
    );
  };

  const itemTemplate = (product, layout, index) => {
    if (!product) {
      return;
    }

    // const donor=await getDonorById(product.donorId);
    if (layout === "list") return listItem(product, index);
    else if (layout === "grid") return gridItem(product);
  };

  const listTemplate = (presents, layout) => {
    return (
      <div className="grid grid-nogutter">
        {presents.map((present, index) => itemTemplate(present, layout, index))}
      </div>
    );
  };

  const openNew = () => {
    setPresent(emptyPresent);
    setSubmitted(false);
    setProductDialog(true);
    setSender("new");
  };
  const searchByName = async (event) => {
    const name = event.target.value;
    if (name == "") {
      await getAllPresents()
    }
    console.log("*******name", name);
    try {
      let tmp = await axios.get(`/Present/SearchByName?name=${name}`).then((res) => {
        console.log(res.data);
        return res.data;

      });
      console.log('tmp', tmp);
      setPresents([...tmp]);
      await GetPresentWithDonor(tmp)

    } catch (err) {
      console.log(err);
    }
  };

  const searchByDonor = async (event) => {
    const name = event.target.value;
    if (name == "") {
      await getAllPresents()
    }
    console.log("*******name", name);
    try {
      let tmp = await axios.get(`/Present/SearchByDonor?donor=${name}`).then((res) => {
        console.log(res.data);
        return res.data;

      });
      console.log('tmp', tmp);
      setPresents([...tmp]);
      await GetPresentWithDonor(tmp)

    } catch (err) {
      console.log(err);
    }
  };

  const searchByNumOfPurcheses = async (event) => {
    const name = event.target.value;
    if (name == "") {
      await getAllPresents()
    }
    console.log("*******name", name);
    try {
      let tmp = await axios.get(`/Present/SearchByNumOfPurcheses?numOfPurcheses=${name}`).then((res) => {
        console.log(res.data);
        return res.data;

      });
      console.log('tmp', tmp);
      setPresents([...tmp]);
      await GetPresentWithDonor(tmp)

    } catch (err) {
      console.log(err);
    }
  };

  const header = () => {
    return (
      <>
        <Button icon="pi pi-plus" label="New" onClick={openNew} />
        <div className="card flex justify-content-center">
          <InputText placeholder="Search By Name" onChange={(e) => searchByName(e)} />
          <InputText placeholder="Search By Donor Name" onChange={(e) => searchByDonor(e)} />
          <InputText placeholder="Search By number of purchases" onChange={(e) => searchByNumOfPurcheses(e)} />
        </div>
        <div className="flex justify-content-end">
          <DataViewLayoutOptions
            layout={layout}
            onChange={(e) => setLayout(e.value)}
          />
        </div>
      </>
    );
  };

  return (
    <>
      <Home activeIndex = {0} />

      <Toast ref={toast} />
      
      <div className="card">
        <DataView
          value={presentDonorList}
          listTemplate={listTemplate}
          layout={layout}
          header={header()}
        />
      </div>
      {selectedDonor ? <>
        {console.log("a", selectedDonor, "b", selectedCategory)}
        <Dialog
          visible={productDialog}
          style={{ width: "32rem" }}
          breakpoints={{ "960px": "75vw", "641px": "90vw" }}
          header="Present Details"
          modal
          className="p-fluid"
          footer={productDialogFooter}
          onHide={hideDialog}
        >
          {present.image && (
            <img
              src={present.image}
              alt={present.image}
              className="donor-image block m-auto pb-3"
            />
          )}

          <div className="field">
            <label htmlFor="name" className="font-bold">
              name
            </label>
            <InputText
              id="name"
              value={present.name}
              onChange={(e) => onInputChange(e, "name")}
              required
              autoFocus
              className={classNames({
                "p-invalid": submitted && !present.name,
              })}
            />
            {submitted && !present.name && (
              <small className="p-error">Name is required.</small>
            )}
          </div>

          <div className="field">
            <label htmlFor="description" className="font-bold block mb-2">
              Description
            </label>
            <InputTextarea
              id="description"
              value={present.description}
              rows={5}
              cols={30}
              onChange={(e) => onInputChange(e, "description")}
              required
              autoFocus
              className={classNames({
                "p-invalid": submitted && !present.description,
              })}
            />
            {submitted && !present.description && (
              <small className="p-error">Description is required.</small>
            )}
          </div>

          <div className="formgrid grid">
            <div className="field col">
              <label htmlFor="name" className="font-bold">
                Price{" "}
              </label>
              {/* <InputNumber inputId="stacked-buttons" id="price" value={present.price} onValueChange={(e) => setValue(e.value)} showButtons mode="currency" currency="USD" onChange={(e) => onInputChange(e, 'price')} required autoFocus className={classNames({ 'p-invalid': submitted && !present.price })} />
                        {submitted && !present.price && <small className="p-error">Price is required.</small>} */}
              <InputText
                id="price"
                value={present.price}
                onChange={(e) => onInputChange(e, "price")}
                required
                autoFocus
                className={classNames({
                  "p-invalid": submitted && !present.price,
                })}
              />
              {submitted && !present.price && (
                <small className="p-error">Price is required.</small>
              )}
            </div>

            <div className="di col">
              <label htmlFor="categoryId" className="font-bold">
                Category
              </label>
              <Dropdown
                value={selectedCategory}
                onChange={(e) => setselectedCategory(e.value)}
                options={CategoryList}
                optionLabel="description"
                showClear
                placeholder="Select a Category"
                className="w-full md:w-14rem"
              />
            </div>

            <div className="di col">
              <label htmlFor="donorId" className="font-bold">
                Donor
              </label>
              <Dropdown
                value={selectedDonor}
                onChange={(e) => setSelectedDonor(e.value)}
                options={DonorsList}
                optionLabel="fullName"
                showClear
                placeholder="Select a Donor"
                className="w-full md:w-14rem"
              />

            </div>

            <div className="field col">
              <label htmlFor="imagePath" className="font-bold">
                ImagePath
              </label>
              <InputText
                id="imagePath"
                value={present.imagePath}
                onChange={(e) => onInputChange(e, "imagePath")}
                required
                autoFocus
                className={classNames({
                  "p-invalid": submitted && !present.imagePath,
                })}
              />
              {submitted && !present.imagePath && (
                <small className="p-error">ImagePath is required.</small>
              )}
            </div>
          </div>
        </Dialog></> : <></>}
    </>
  );
}
