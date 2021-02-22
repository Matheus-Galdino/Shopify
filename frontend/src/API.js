async function basicFectch(url) {
  const result = await fetch(url);
  const json = await result.json();
  return json;
}

export default {
  getItems: async () => {
    return await basicFectch("https://localhost:44336/item");
  },
  getCategories: async () => {
    return await basicFectch("https://localhost:44336/category");
  },
  getAllLists: async (grouped) => {
    return await basicFectch(
      `https://localhost:44336/shoplist?grouped=${grouped}`
    );
  },
  getActiveList: async () => {
    const result = await fetch(`https://localhost:44336/shoplist/active`);

    if (result.status === 204) return {};

    return await result.json();
  },
  getShoppingList: async (listId) => {
    return await basicFectch(`https://localhost:44336/shoplist/${listId}`);
  },
  createItem: async (item) => {
    const result = await fetch("https://localhost:44336/item/new", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(item),
    });

    if (!result.ok) {
      let errorText = await result.text();
      return new Error(errorText);
    }
  },
  createCategory: async (name) => {
    const body = JSON.stringify({ name });
    const result = await fetch("https://localhost:44336/category/new", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: body,
    });

    if (result.status == 400) return new Error(await result.text());
  },
  deleteItem: async (itemId) => {
    const result = fetch(`https://localhost:44336/item/${itemId}`, {
      method: "DELETE",
    });

    return result;
  },
  addItemToList: async (listId, itemId) => {
    const result = await fetch(
      `https://localhost:44336/shoplist/${listId}/add`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ id: itemId, quantity: 1 }),
      }
    );

    if (result.status == 400) return new Error("Item is already on the list");
  },
  removeItemFromList: async (listId, itemId) => {
    await fetch(`https://localhost:44336/shoplist/${listId}/${itemId}`, {
      method: "DELETE",
    });
  },
  updateItemQuantity: async (listId, itemId, quantity) => {
    await fetch(
      `https://localhost:44336/shoplist/${listId}/${itemId}?quantity=${quantity}`,
      {
        method: "PUT",
      }
    );
  },
  changeListItemStatus: async (listId, itemId, isCompleted) => {
    await fetch(
      `https://localhost:44336/shoplist/${listId}/${itemId}/${isCompleted}`,
      {
        method: "PUT",
      }
    );
  },
  createShoppingList: async (name, date) => {
    const json = JSON.stringify({
      name,
      date,
    });

    const result = await fetch("https://localhost:44336/shoplist/new", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: json,
    });

    if (result.status === 400) return new Error(await result.text());
  },
  updateShoppingList: async (listId, name) => {
    await fetch(`https://localhost:44336/shoplist/${listId}?name=${name}`, {
      method: "PUT",
    });
  },
  changeListStatus: async (listId, status) => {
    await fetch(`https://localhost:44336/shoplist/${listId}/${status}`, {
      method: "PATCH",
    });
  },
  setActiveList: async (listId) => {
    await fetch(`https://localhost:44336/shoplist/active/${listId}`, {
      method: "PUT",
    });
  },
  deleteList: async (listId) => {
    await fetch(`https://localhost:44336/shoplist/${listId}`, {
      method: "DELETE",
    });
  },
  getTopItems: async () => {
    return await basicFectch("https://localhost:44336/stats/items");
  },
  getTopCategories: async () => {
    return await basicFectch("https://localhost:44336/stats/categories");
  },
  getMonthlySummary: async () => {
    return await basicFectch("https://localhost:44336/stats/monthly");
  },
};
