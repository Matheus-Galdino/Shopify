<template>
  <div id="shopping-list-details">
    <header>
      <button @click="$emit('back')">
        <span class="material-icons"> keyboard_backspace </span>
        back
      </button>
    </header>

    <h2>{{ detailedList.name }}</h2>
    <small>
      <span class="material-icons"> event_note </span>
      {{ new Date(detailedList.date).toDateString() }}
    </small>

    <div class="list-items">
      <section
        class="category-group"
        v-for="group in detailedList.groups"
        :key="group.name"
      >
        <h3>{{ group.name }}</h3>
        <div class="items">
          <div class="item" v-for="item in group.items" :key="item.id">
            <p>{{ item.name }}</p>
            <span>{{ item.quantity }} pcs</span>
          </div>
        </div>
      </section>
    </div>
  </div>
</template>

<script>
import API from "@/API";

export default {
  name: "ShoppingListDetails",
  props: {
    shoppingListId: {
      type: Number,
      required: true,
    },
  },
  data() {
    return {
      detailedList: {},
    };
  },
  async beforeMount() {
    this.detailedList = await API.getShoppingList(this.shoppingListId);    
  },
};
</script>

<style scoped>
header {
  margin-bottom: 3.5rem;
}

header button {
  display: flex;
  column-gap: 5px;
  align-items: center;

  border: none;
  color: #f9a109;
  background: transparent;

  cursor: pointer;
}

h2 {
  color: #34333a;
  font-size: 3rem;
  margin-bottom: 2rem;
}

small {
  display: flex;
  column-gap: 1rem;
  font-size: 12px;
  color: #c1c1c4;
  font-weight: 500;
  align-items: center;
}

.list-items {
  margin-top: 5.5rem;
}

.category-group {
  margin-bottom: 6.5rem;
}

.category-group h3 {
  font-weight: 600;
  font-size: 1.8rem;
  margin-bottom: 2rem;
}

.items {
  display: flex;
  flex-wrap: wrap;
  column-gap: 2rem;
  row-gap: 4.5rem;
  align-items: center;
}

.items .item {
  display: flex;
  column-gap: 3rem;
  align-items: center;
  justify-content: space-between;

  padding: 1.5rem;

  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0px 2px 12px rgba(0, 0, 0, 0.05);
}

.item p {
  font-weight: 500;
  font-size: 1.6rem;
}

.item span {
  color: #f9a10a;
  font-weight: bold;
  font-size: 1.2rem;
}
</style>
