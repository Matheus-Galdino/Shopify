<template>
  <div class="item" @mouseleave="isActionGroupVisible = false">
    <div class="name-container">
      <Checkbox
        v-show="isInStats"
        borderColor="#F9A109"
        iconColor="#F9A109"
        :isChecked="item.isCompleted"
        @click="changeCompleted"
      />
      <p :class="{ completed: item.isCompleted }">
        {{ item.name }}
      </p>
    </div>

    <div :class="{ 'actions-group': isActionGroupVisible }">
      <button
        class="delete"
        v-show="isActionGroupVisible && !isInStats"
        @click="removeFromList"
      >
        <span class="material-icons"> delete_outline </span>
      </button>
      <span
        class="material-icons"
        v-show="isActionGroupVisible && !isInStats"
        @click="reduceQuantity"
      >
        remove
      </span>
      <button class="quantity-button" @click="changeActionGroupVisible">
        {{ item.quantity }} pcs
      </button>
      <span
        class="material-icons"
        v-show="isActionGroupVisible && !isInStats"
        @click="increaseQuantity"
      >
        add
      </span>
    </div>
  </div>
</template>

<script>
import API from "@/API";
import Checkbox from "./Checkbox.vue";

export default {
  components: { Checkbox },
  name: "ShoppingListItem",
  props: {
    baseItem: {
      type: Object,
      required: true,
    },
    listId: {
      type: Number,
      required: true,
    },
    isInStats: {
      type: Boolean,
    },
  },
  data() {
    return {      
      item: this.baseItem,
      isActionGroupVisible: false,
    };
  },
  methods: {
    async removeFromList() {
      await API.removeItemFromList(this.listId, this.item.id);
      this.$emit("has-removed-item");
    },    
    async reduceQuantity() {
      this.hasChanged = true;
      this.item.quantity -= 1;

      if (this.item.quantity <= 0) this.removeFromList();
      else await API.updateItemQuantity(this.listId, this.item.id, this.item.quantity);
      console.log(`Reduced quantity from item: ${this.item.name}`);

      this.$emit('refresh-chart');
    },
    async increaseQuantity() {
      this.item.quantity += 1;
      await API.updateItemQuantity(this.listId, this.item.id, this.item.quantity);      
      this.$emit('refresh-chart');
    },    
    async changeCompleted() {
      this.item.isCompleted = !this.item.isCompleted;
      console.log(`changing status for item: ${this.item.name} with value: ${this.item.isCompleted}`);
      await API.changeListItemStatus(
        this.listId,
        this.item.id,
        this.item.isCompleted
      );
    },
    changeActionGroupVisible() {
      if (!this.isInStats) this.isActionGroupVisible = true;
    },
  },
};
</script>

<style scoped>
.item {
  display: flex;
  align-items: center;
  justify-content: space-between;

  margin-bottom: 1.5rem;
}

.item .name-container {
  display: flex;
  align-items: center;
  column-gap: 1rem;
}

.item p {
  font-weight: 500;
  font-size: 1.8rem;
}

.item p.completed {
  text-decoration: line-through;
}

.category-group .item .actions-group {
  display: flex;
  align-items: center;
  background: #ffffff;
  border-radius: 12px;
  column-gap: 0.5rem;
  padding-right: 1rem;
}

.actions-group .delete {
  border: none;
  background: #f9a109;
  border-radius: 12px;
  padding: 0.7rem;
}

.actions-group .delete .material-icons {
  color: #fff;
}

.actions-group .material-icons {
  cursor: pointer;
  color: #f9a109;
}

.quantity-button {
  color: #f9a109;
  background: transparent;

  border: 2px solid #f9a109;
  border-radius: 24px;

  padding: 0.8rem 2rem;
  margin: 0.6rem 0;
  cursor: pointer;
}
</style>
